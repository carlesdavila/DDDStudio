namespace DDDCanvasCreator.Services;

public interface IYamlProcessor
{
    void ProcessYamlAndGenerateSvg(string yamlContent, string outputFilePath);
}
