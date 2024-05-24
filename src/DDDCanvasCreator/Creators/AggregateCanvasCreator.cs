using DDDCanvasCreator.Models.AggregateCanvas;
using DDDCanvasCreator.Services;
using YamlDotNet.Serialization;

namespace DDDCanvasCreator.Creators;

public class AggregateCanvasCreator : IYamlProcessor
{
    public void ProcessYamlAndGenerateSvg(string yamlContent, string outputFilePath)
    {
        var aggregate = ParseYaml(yamlContent);
        GenerateAggregateSvg(aggregate, outputFilePath);
    }

    private Aggregate ParseYaml(string yamlContent)
    {
        var deserializer = new DeserializerBuilder().Build();
        return deserializer.Deserialize<Aggregate>(yamlContent);
    }

    private void GenerateAggregateSvg(Aggregate aggregate, string outputFilePath)
    {
        throw new NotImplementedException();
    }
}