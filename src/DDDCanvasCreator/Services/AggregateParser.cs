using DDDCanvasCreator.Models.AggregateCanvas;
using YamlDotNet.RepresentationModel;

namespace DDDCanvasCreator.Services;

public static class AggregateParser
{
    public static void HandleAggregate(YamlMappingNode yamlMappingNode, Aggregate aggregate)
    {
        foreach (var child in yamlMappingNode.Children)
        {
            var key = YamlParser.GetScalarValue(child.Key);

            switch (key)
            {
                case "aggregate":
                    YamlParser.ThrowIfNotYamlMapping(child.Value);
                    HandleAggregateMapping(child.Value as YamlMappingNode, aggregate);
                    break;
                default:
                    throw new DDDYamlException(child.Key.Start, $"Unrecognized Key: {key}");
            }
        }
    }

    private static void HandleAggregateMapping(YamlMappingNode? yamlMappingNode, Aggregate aggregate)
    {
        foreach (var child in yamlMappingNode!.Children)
        {
            var key = YamlParser.GetScalarValue(child.Key);

            switch (key)
            {
                case "name":
                    aggregate.Name = YamlParser.GetScalarValue(key, child.Value);
                    break;
                case "description":
                    aggregate.Description = YamlParser.GetScalarValue(key, child.Value);
                    break;
                default:
                    throw new DDDYamlException(child.Key.Start, $"Unrecognized Key: {key}");
            }
        }
    }
}