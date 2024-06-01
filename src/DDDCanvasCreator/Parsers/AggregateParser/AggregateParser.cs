using DDDCanvasCreator.Models.AggregateCanvas;
using DDDCanvasCreator.Services;
using YamlDotNet.RepresentationModel;

namespace DDDCanvasCreator.Parsers.AggregateParser;

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

    private static void HandleAggregateMapping(YamlMappingNode? yamlMappingNode, Models.AggregateCanvas.Aggregate aggregate)
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
                case "stateTransitions":
                    YamlParser.ThrowIfNotYamlSequence(key, child.Value);
                    StateTransitionsParser.HandleStateTransitions((child.Value as YamlSequenceNode)!, aggregate.StateTransitions);
                    break;
                default:
                    throw new DDDYamlException(child.Key.Start, $"Unrecognized Key: {key}");
            }
        }
    }
}