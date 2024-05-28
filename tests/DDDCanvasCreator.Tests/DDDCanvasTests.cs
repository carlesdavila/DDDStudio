using System.Reflection;

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
        var outputFilePath = "./bounded_context_output.svg";

        // Act
        dddCanvas.GenerateSvg(yamlFilePath, outputFilePath);

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
        var outputFilePath = "./aggregate_output.svg";

        // Act
        dddCanvas.GenerateSvg(yamlFilePath, outputFilePath);

        // Assert
        Assert.True(File.Exists(outputFilePath));
    }
}