using static Bullseye.Targets;
using static SimpleExec.Command;

const string artifactsDir = "artifacts";

Target(Targets.RestoreTools, async () =>
{
    await RunAsync("dotnet", "tool restore");
});

Target(Targets.CleanArtifactsOutput, () =>
{
    if (Directory.Exists(artifactsDir)) Directory.Delete(artifactsDir, true);
});

Target(Targets.CleanBuildOutput, async () =>
{
    await RunAsync("dotnet", "clean -c Release -v m --nologo");
});

Target(Targets.Build, DependsOn(Targets.CleanBuildOutput), async () =>
{
    await RunAsync("dotnet", "build -c Release --nologo");
});

Target(Targets.RunTests, DependsOn(Targets.Build), async () =>
{
    await RunAsync("dotnet", "test -c Release --no-build --nologo");
});

Target(Targets.Pack, DependsOn(Targets.CleanArtifactsOutput, Targets.Build), async () =>
{
    await RunAsync("dotnet", $"pack -c Release -o {Directory.CreateDirectory(artifactsDir).FullName} --no-build --nologo");
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
    public const string PublishArtifacts = "publish-artifacts";
}