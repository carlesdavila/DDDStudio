using ddd.Commands;

namespace ddd.Tests.Commands;

public class AddSubdomainCommandTests
{
    [Fact]
    public void Subdomain_CreatesNewSubdomainAndYamlFile_WhenSubdomainDoesNotExist()
    {
        // Arrange
        var command = new AddSubdomainCommand();
        const string subdomainName = "TestSubdomain";
        var subdomainPath = Path.Combine(Constants.MainPath, subdomainName);
        var yamlFilePath = Path.Combine(subdomainPath, $"{subdomainName}.yaml");

        // Cleanup any pre-existing test artifacts
        if (Directory.Exists(subdomainPath)) Directory.Delete(subdomainPath, true);

        // Act
        command.Subdomain(subdomainName);

        // Assert
        Assert.True(Directory.Exists(subdomainPath));
        Assert.True(File.Exists(yamlFilePath));
        var yamlContent = File.ReadAllText(yamlFilePath);
        Assert.Contains("boundedContexts", yamlContent);

        // Cleanup
        Directory.Delete(subdomainPath, true);
    }

    [Fact]
    public void Subdomain_DoesNotCreateSubdomain_WhenSubdomainAlreadyExists()
    {
        // Arrange
        var command = new AddSubdomainCommand();
        var subdomainName = "ExistingSubdomain";
        var subdomainPath = Path.Combine(Constants.MainPath, subdomainName);
        Directory.CreateDirectory(subdomainPath); // Pre-create the subdomain

        try
        {
            using var sw = new StringWriter();
            Console.SetOut(sw);

            // Act
            command.Subdomain(subdomainName);

            // Assert
            var output = sw.ToString().Trim();
            Assert.Contains($"Subdomain '{subdomainName}' already exists.", output);
        }
        finally
        {
            // Cleanup
            if (Directory.Exists(subdomainPath)) Directory.Delete(subdomainPath, true);
        }
    }
}