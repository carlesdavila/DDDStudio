using DDDCanvasCreator.Models.BoundedContextBasic;
using YamlDotNet.RepresentationModel;

namespace DDDCanvasCreator.Services;

public static class BoundedContextsBasicParser
{
    public static void HandleBoundedContextsBasic(YamlMappingNode yamlMappingNode,  BoundedContextsBasic app)
    {
        foreach (var child in yamlMappingNode.Children)
        {
            var key = YamlParser.GetScalarValue(child.Key);

            switch (key)
            {
                case "boundedContexts":
                    YamlParser.ThrowIfNotYamlSequence(key, child.Value);
                    BoundedContextsParser.HandleBoundedContexts((child.Value as YamlSequenceNode)!, app.BoundedContexts);
                    break;
                default:
                    throw new DDDYamlException(child.Key.Start, $"Unrecognized Key: {key}");
            }
        }
    }

}