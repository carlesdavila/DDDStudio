using DDDCanvasCreator.Models;

namespace DDDCanvasCreator.Services;

public interface IYamlProcessor
{
    void ProcessYamlAndGenerateSvg(string yamlContent, string outputFilePath, DddConfig config);
}
