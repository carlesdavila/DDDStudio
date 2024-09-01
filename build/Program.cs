using System.Text.RegularExpressions;
using static Bullseye.Targets;
using static SimpleExec.Command;

const string artifactsDir = "artifacts";
const string toolName = "DDD.Studio";


Target(Targets.RestoreTools, async () => { await RunAsync("dotnet", "tool restore"); });

Target(Targets.CleanArtifactsOutput, () =>
{
    if (Directory.Exists(artifactsDir)) Directory.Delete(artifactsDir, true);
});

Target(Targets.CleanBuildOutput, async () => { await RunAsync("dotnet", "clean -c Release -v m --nologo"); });

Target(Targets.Build, DependsOn(Targets.CleanBuildOutput),
    async () => { await RunAsync("dotnet", "build -c Release --nologo"); });

Target(Targets.RunTests, DependsOn(Targets.Build),
    async () => { await RunAsync("dotnet", "test -c Release --no-build --nologo"); });

Target(Targets.Pack, DependsOn(Targets.CleanArtifactsOutput, Targets.Build),
    async () =>
    {
        await RunAsync("dotnet",
            $"pack ./src/ddd/ddd.csproj -c Release -o {Directory.CreateDirectory(artifactsDir).FullName} --no-build --nologo");
    });


Target(Targets.UninstallPackage, async () =>
{
    try
    {
        // Check if the tool is installed
        await RunAsync("dotnet", $"tool list -g | findstr {toolName}");
        
        // If the tool is found, uninstall it
        await RunAsync("dotnet", $"tool uninstall -g {toolName}");
    }
    catch
    {
        // If the tool is not found, skip uninstallation
        Console.WriteLine($"{toolName} is not installed, skipping uninstallation.");
    }
});

Target(Targets.InstallPackage, DependsOn(Targets.UninstallPackage, Targets.Pack), async () =>
{
    var nugetConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "nuget.config");

    // Create nuget.config file
    var nugetConfigContent = $@"
<configuration>
  <packageSources>
    <clear />
    <add key=""local"" value=""{Path.GetFullPath(artifactsDir)}"" />
  </packageSources>
</configuration>";
    await File.WriteAllTextAsync(nugetConfigPath, nugetConfigContent);

    try
    {

        var minverOutput = await ReadAsync("minver");
        var versionMatch = Regex.Match(minverOutput.StandardOutput, @"Calculated version (\S+)");
        var version = versionMatch.Success ? versionMatch.Groups[1].Value : throw new Exception("Version not found in minver output");
        version = version.Replace("alpha", "preview");

        
        // Install the tool using the nuget.config file
        await RunAsync("dotnet", $"tool install --global --add-source ./{artifactsDir} {toolName} --version {version} --ignore-failed-sources");
    }
    finally
    {
        // Delete the nuget.config file
        if (File.Exists(nugetConfigPath))
        {
            File.Delete(nugetConfigPath);
        }
    }
});

Target(Targets.TestDddTool, DependsOn(Targets.InstallPackage), async () =>
{
    var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    Directory.CreateDirectory(tempDir);

    Console.WriteLine($"Using temporary directory: {tempDir}");

    try
    {
        // Initialize a DDD project
        await RunAsync("ddd", "init", tempDir);

        // Add a subdomain
        await RunAsync("ddd", "add subdomain Product", tempDir);

        // Add a bounded context to the subdomain
        await RunAsync("ddd", "add context -s Product Product", tempDir);

        // Add an aggregate to the bounded context
        await RunAsync("ddd", "add aggregate -s Product -c Product Product", tempDir);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error executing DDD tool commands: {ex.Message}");
    }
    // finally
    // {
    //     // Clean up the temporary directory
    //     if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
    // }
});


Target(Targets.PublishArtifacts, DependsOn(Targets.Pack), () => Console.WriteLine("publish artifacts"));

Target("default", DependsOn(Targets.RunTests, Targets.PublishArtifacts));

await RunTargetsAndExitAsync(args);

internal static class Targets
{
    public const string RestoreTools = "restore-tools";
    public const string CleanBuildOutput = "clean-build-output";
    public const string CleanArtifactsOutput = "clean-artifacts-output";

    public const string Build = "build";
    public const string RunTests = "run-tests";

    public const string Pack = "pack";
    public const string UninstallPackage = "uninstall-package";
    public const string InstallPackage = "install-package";
    public const string PublishArtifacts = "publish-artifacts";
    public const string TestDddTool = "test-ddd-tool";
}