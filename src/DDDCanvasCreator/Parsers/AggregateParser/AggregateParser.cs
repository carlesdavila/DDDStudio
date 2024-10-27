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
                case "stateTransitions":
                    if (child.Value.NodeType == YamlNodeType.Sequence)
                    {
                        YamlParser.ThrowIfNotYamlSequence(key, child.Value);
                        StateTransitionsParser.HandleStateTransitions((child.Value as YamlSequenceNode)!,
                            aggregate.StateTransitions);
                    }

                    break;
                case "enforcedInvariants":
                    if (child.Value.NodeType == YamlNodeType.Sequence)
                    {
                        YamlParser.ThrowIfNotYamlSequence(key, child.Value);
                        HandleStringSequence(child.Value as YamlSequenceNode, aggregate.EnforcedInvariants);
                    }

                    break;
                case "correctivePolicies":
                    if (child.Value.NodeType == YamlNodeType.Sequence)
                    {
                        YamlParser.ThrowIfNotYamlSequence(key, child.Value);
                        HandleStringSequence(child.Value as YamlSequenceNode, aggregate.CorrectivePolicies);
                    }

                    break;
                case "handledCommands":
                    if (child.Value.NodeType == YamlNodeType.Sequence)
                    {
                        YamlParser.ThrowIfNotYamlSequence(key, child.Value);
                        HandleStringSequence(child.Value as YamlSequenceNode, aggregate.HandledCommands);
                    }

                    break;
                case "createdEvents":
                    if (child.Value.NodeType == YamlNodeType.Sequence)
                    {
                        YamlParser.ThrowIfNotYamlSequence(key, child.Value);
                        HandleStringSequence(child.Value as YamlSequenceNode, aggregate.CreatedEvents);
                    }

                    break;
                default:
                    throw new DDDYamlException(child.Key.Start, $"Unrecognized Key: {key}");
            }
        }
    }

    //TODO: Move to YamlParser?
    private static void HandleStringSequence(YamlSequenceNode? yamlSequenceNode, List<string> enforcedInvariants)
    {
        foreach (var child in yamlSequenceNode!.Children)
        {
            var enforcedInvariant = YamlParser.GetScalarValue(child);
            enforcedInvariants.Add(enforcedInvariant);
        }
    }
}