using DDDCanvasCreator.Models.BoundedContextBasic;
using YamlDotNet.RepresentationModel;

namespace DDDCanvasCreator.Services;

public static class BoundedContextsParser
{
    public static void HandleBoundedContexts(YamlSequenceNode yamlSequenceNode, List<BoundedContext> boundedContexts)
    {
        foreach (var child in yamlSequenceNode.Children)
        {
            YamlParser.ThrowIfNotYamlMapping(child);
            var boundedContext = new BoundedContext();
            HandleBoundedContextMapping((YamlMappingNode)child, boundedContext);
            boundedContexts.Add(boundedContext);
        }
    }

    private static void HandleBoundedContextMapping(YamlMappingNode yamlMappingNode, BoundedContext boundedContext)
    {
        foreach (var child in yamlMappingNode!.Children)
        {
            var key = YamlParser.GetScalarValue(child.Key);

            switch (key)
            {
                case "name":
                    boundedContext.Name = YamlParser.GetScalarValue(key, child.Value).ToLowerInvariant();
                    break;
                case "color":
                    boundedContext.Color = YamlParser.GetScalarValue(key, child.Value).ToLowerInvariant();
                    break;
                case "models":
                    YamlParser.ThrowIfNotYamlSequence(key, child.Value);
                    ModelsParser.HandleModels((child.Value as YamlSequenceNode)!, boundedContext.Models);
                    break;
                default:
                    throw new DDDYamlException(child.Key.Start, $"Unrecognized Key: {key}");
            }
        }
    }
}