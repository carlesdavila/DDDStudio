using System.Reflection;
using DDDCanvasCreator.Models;

namespace DDDCanvasCreator.Tests;

public class DDDCanvasTests
{
    [Fact]
    public void GenerateBoundedContextSvg_WhenYamlFileExists_GeneratesSvg()
    {
        // Arrange
        
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var directory = Path.GetDirectoryName(assemblyLocation);
        var yamlFilePath = Path.Combine(directory!, "TestData", "bounded_context_basic_input.yaml");
        
        var dddCanvas = new DDDCanvas();
        const string outputFilePath = "./bounded_context_output.svg";

        // Act
        dddCanvas.GenerateSvg(yamlFilePath, outputFilePath, new DddConfig());

        // Assert
        Assert.True(File.Exists(outputFilePath));
    }

    [Fact]
    public void GenerateAggregateSvg_WhenYamlFileExists_GeneratesSvg()
    {
        // Arrange
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var directory = Path.GetDirectoryName(assemblyLocation);
        var yamlFilePath = Path.Combine(directory!, "TestData", "aggregate_input.yaml");
        
        var dddCanvas = new DDDCanvas();
        const string outputFilePath = "./aggregate_output.svg";

        // Act
        dddCanvas.GenerateSvg(yamlFilePath, outputFilePath, new DddConfig());

        // Assert
        Assert.True(File.Exists(outputFilePath));
    }
    
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
        Assert.Equal(["#111111", "#222222", "#333333"], config.AggregateStatesColors);
        Assert.Equal("#444444", config.HandledCommandsColor);
        Assert.Equal("#555555", config.CreatedEventsColor);
        Assert.Equal(["#666666", "#777777", "#888888"], config.BoundedContextColors);

        // Clean up
        File.Delete(configFilePath);
    }
    
    [Fact]
    public void GenerateSubdomainsSvg_WhenYamlFileExists_GeneratesSvg()
    {
        // Arrange
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var directory = Path.GetDirectoryName(assemblyLocation);
        var yamlFilePath = Path.Combine(directory!, "TestData", "subdomains_input.yaml");
        
        var dddCanvas = new DDDCanvas();
        const string outputFilePath = "./subdomains_output.svg";

        // Act
        dddCanvas.GenerateSvg(yamlFilePath, outputFilePath, new DddConfig());

        // Assert
        Assert.True(File.Exists(outputFilePath));
    }
}