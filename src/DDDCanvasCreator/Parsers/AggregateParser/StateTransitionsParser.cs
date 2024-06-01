using DDDCanvasCreator.Models.AggregateCanvas;
using DDDCanvasCreator.Services;
using YamlDotNet.RepresentationModel;

namespace DDDCanvasCreator.Parsers.AggregateParser;

internal class StateTransitionsParser
{
    public static void HandleStateTransitions(YamlSequenceNode yamlSequenceNode, List<StateTransition> aggregateStateTransitions)
    {
        foreach (var child in yamlSequenceNode.Children)
        {
            YamlParser.ThrowIfNotYamlMapping(child);
            var stateTransition = new StateTransition();
            HandleStateTransitionMapping((YamlMappingNode)child, stateTransition);
            aggregateStateTransitions.Add(stateTransition);
        }
    }

    private static void HandleStateTransitionMapping(YamlMappingNode yamlMappingNode, StateTransition stateTransition)
    {
        foreach (var child in yamlMappingNode!.Children)
        {
            var key = YamlParser.GetScalarValue(child.Key);

            switch (key)
            {
                case "state":
                    stateTransition.State = YamlParser.GetScalarValue(key, child.Value);
                    break;
                case "transitions":
                    YamlParser.ThrowIfNotYamlSequence(key, child.Value);
                    TosParser.HandleTos((child.Value as YamlSequenceNode)!, stateTransition.Transitions);
                    break;
                default:
                    throw new DDDYamlException(child.Key.Start, $"Unrecognized Key: {key}");
            }
        }
    }
}