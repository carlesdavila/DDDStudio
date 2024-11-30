using System.Reflection;
using DDDCanvasCreator.Models;

namespace DDDCanvasCreator.Tests;

public class DDDCanvasBoundedContextTests
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
    public void GenerateBoundedContextSvg_WhenYamlFileHasLongNames_GeneratesSvg()
    {
        // Arrange
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var directory = Path.GetDirectoryName(assemblyLocation);
        var yamlFilePath = Path.Combine(directory!, "TestData", "bounded_context_basic_input2.yaml");

        var dddCanvas = new DDDCanvas();
        const string outputFilePath = "./bounded_context_output_long_names.svg";

        // Act
        dddCanvas.GenerateSvg(yamlFilePath, outputFilePath, new DddConfig());

        // Assert
        Assert.True(File.Exists(outputFilePath));
    }
}