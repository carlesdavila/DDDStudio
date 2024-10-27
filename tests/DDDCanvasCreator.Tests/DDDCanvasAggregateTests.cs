using System.Reflection;
using DDDCanvasCreator.Models;

namespace DDDCanvasCreator.Tests;

public class DDDCanvasAggregateTests
{
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
    public void GenerateAggregateSvg_WhenYamlFileHasEmptyProperties_GeneratesSvgWithoutEmptyProperties()
    {
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var directory = Path.GetDirectoryName(assemblyLocation);
        var yamlFilePath = Path.Combine(directory!, "TestData", "aggregate_empty.yaml");

        var dddCanvas = new DDDCanvas();
        const string outputFilePath = "./aggregate_output2.svg";

        dddCanvas.GenerateSvg(yamlFilePath, outputFilePath, new DddConfig());

        Assert.True(File.Exists(outputFilePath));
    }
}
