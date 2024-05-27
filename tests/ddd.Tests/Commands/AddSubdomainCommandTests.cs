using Cocona;
using ddd.Commands;

namespace ddd.Tests.Commands;

public class AddSubdomainCommandTests
{
    [Fact(Skip = "review")]
    public void AddSubdomain_CommandExecutes_Successfully()
    {
        // Arrange
        var args = new[] { "add-subdomain", "TestSubdomain" };
        CoconaApp.Run<InitCommand>();

        // Act
        CoconaApp.Run<AddSubdomainCommand>(args);

        // Assert
        Assert.True(Directory.Exists(Path.Combine("DDD", "Subdomains", "TestSubdomain")));
    }
}