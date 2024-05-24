namespace DDDCanvasCreator.Tests;

public class DDDCanvasTests
{
    [Fact]
    public void GenerateBoundedContextSvg_WhenYamlFileExists_GeneratesSvg()
    {
        // Arrange
        var dddCanvas = new DDDCanvas();
        var yamlFilePath = "path/to/your/bounded_context_input.yaml";
        var outputFilePath = "path/to/your/bounded_context_output.svg";

        // Act
        dddCanvas.GenerateSvg(yamlFilePath, outputFilePath);

        // Assert
        Assert.True(File.Exists(outputFilePath));
    }

    [Fact]
    public void GenerateAggregateSvg_WhenYamlFileExists_GeneratesSvg()
    {
        // Arrange
        var dddCanvas = new DDDCanvas();
        var yamlFilePath = "path/to/your/aggregate_input.yaml";
        var outputFilePath = "path/to/your/aggregate_output.svg";

        // Act
        dddCanvas.GenerateSvg(yamlFilePath, outputFilePath);

        // Assert
        Assert.True(File.Exists(outputFilePath));
    }
}