using ddd.Commands;
using Xunit.Sdk;

namespace ddd.Tests.Commands;

public class AddContextCommandTests
{
    [Fact(Skip = "review")]
    public void Context_CreatesNewContext_WhenContextDoesNotExist()
    {
        // Arrange
        var command = new AddContextCommand();
        const string contextName = "TestContext";
        const string subdomainName = "TestSubdomain";
        var contextPath = Path.Combine(Constants.MainPath, subdomainName, contextName);

        // Cleanup any pre-existing test artifacts
        if (Directory.Exists(Constants.MainPath))
        {
            Directory.Delete(Constants.MainPath, true);
        }

        // Act
        command.Context(contextName, subdomainName);

        // Assert
        Assert.True(Directory.Exists(contextPath));

        // Cleanup
        Directory.Delete(Constants.MainPath, true);
    }
}