using DDDCanvasCreator.Models.AggregateCanvas;
using DDDCanvasCreator.Services;
using YamlDotNet.RepresentationModel;

namespace DDDCanvasCreator.Parsers.AggregateParser;

internal class TosParser
{
    public static void HandleTos(YamlSequenceNode yamlSequenceNode, List<Transition> transitions)
    {
        foreach (var child in yamlSequenceNode.Children)
        {
            YamlParser.ThrowIfNotYamlMapping(child);
            var transition = new Transition();
            HandleTosMapping((YamlMappingNode)child, transition);
            transitions.Add(transition);
        }
    }

    private static void HandleTosMapping(YamlMappingNode yamlMappingNode, Transition transition)
    {
        foreach (var child in yamlMappingNode!.Children)
        {
            var key = YamlParser.GetScalarValue(child.Key);

            transition.To = key switch
            {
                "to" => YamlParser.GetScalarValue(key, child.Value),
                _ => throw new DDDYamlException(child.Key.Start, $"Unrecognized Key: {key}")
            };
        }
    }
}