using DDDCanvasCreator.Services;

namespace DDDCanvasCreator;

public class DDDCanvas
{
    public void GenerateSvg(string yamlFilePath, string outputFilePath)
    {
        if (string.IsNullOrEmpty(yamlFilePath) || !File.Exists(yamlFilePath))
            throw new FileNotFoundException("The YAML file was not found.", yamlFilePath);

        var yamlContent = File.ReadAllText(yamlFilePath);
        var processor = YamlProcessorFactory.CreateProcessor(yamlContent);
        processor.ProcessYamlAndGenerateSvg(yamlContent, outputFilePath);
    }
}