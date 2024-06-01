using DDDCanvasCreator.Models.BoundedContextBasic;
using DDDCanvasCreator.Services;
using YamlDotNet.RepresentationModel;

namespace DDDCanvasCreator.Parsers.BoundedContextsParser;

public static class BoundedContextsBasicParser
{
    public static void HandleBoundedContextsBasic(YamlMappingNode yamlMappingNode,  BoundedContextsBasic boundedContextsBasic)
    {
        foreach (var child in yamlMappingNode.Children)
        {
            var key = YamlParser.GetScalarValue(child.Key);

            switch (key)
            {
                case "boundedContexts":
                    YamlParser.ThrowIfNotYamlSequence(key, child.Value);
                    BoundedContextsParser.HandleBoundedContexts((child.Value as YamlSequenceNode)!, boundedContextsBasic.BoundedContexts);
                    break;
                default:
                    throw new DDDYamlException(child.Key.Start, $"Unrecognized Key: {key}");
            }
        }
    }

}