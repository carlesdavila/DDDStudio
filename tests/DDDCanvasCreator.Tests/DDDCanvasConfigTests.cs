using DDDCanvasCreator.Models;

namespace DDDCanvasCreator.Tests;

public class DDDCanvasConfigTests
{
    [Fact]
    public void LoadConfig_ShouldLoadConfigurationFromFile()
    {
        // Arrange
        const string configFilePath = "test-config.yaml";
        const string configFileContent = @"
        AggregateBase: ""TestNamespace.IAggregateRoot""
        AggregateStatesColors:
          - ""#111111""
          - ""#222222""
          - ""#333333""
        HandledCommandsColor: ""#444444""
        CreatedEventsColor: ""#555555""
        BoundedContextColors:
          - ""#666666""
          - ""#777777""
          - ""#888888""";

        File.WriteAllText(configFilePath, configFileContent);

        // Act
        var config = DDDCanvas.LoadConfig(configFilePath);

        // Assert
        Assert.Equal("TestNamespace.IAggregateRoot", config.AggregateBase);
        Assert.Equal(new[] { "#111111", "#222222", "#333333" }, config.AggregateStatesColors);
        Assert.Equal("#444444", config.HandledCommandsColor);
        Assert.Equal("#555555", config.CreatedEventsColor);
        Assert.Equal(new[] { "#666666", "#777777", "#888888" }, config.BoundedContextColors);

        // Clean up
        File.Delete(configFilePath);
    }
}