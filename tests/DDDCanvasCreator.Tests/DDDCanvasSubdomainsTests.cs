using System.Reflection;
using DDDCanvasCreator.Models;

namespace DDDCanvasCreator.Tests;

public class DDDCanvasSubdomainsTests
{
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