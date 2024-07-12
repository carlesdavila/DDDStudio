using DDDCanvasCreator.Services;

namespace DDDCanvasCreator;

public class DDDCanvas
{
    public void GenerateSvg(string yamlFilePath, string outputFilePath)
    {
        if (string.IsNullOrEmpty(yamlFilePath))
            throw new FileNotFoundException("File Path is null or empty", yamlFilePath);

        if (!File.Exists(yamlFilePath))
            throw new Exception($"The YAML file was not found. {yamlFilePath}");

        var yamlContent = File.ReadAllText(yamlFilePath);
        var processor = YamlProcessorFactory.CreateProcessor(yamlContent);
        processor.ProcessYamlAndGenerateSvg(yamlContent, outputFilePath);
    }
}