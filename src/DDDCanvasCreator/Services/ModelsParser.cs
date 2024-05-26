using DDDCanvasCreator.Models.BoundedContextBasic;
using YamlDotNet.RepresentationModel;

namespace DDDCanvasCreator.Services;

public static class ModelsParser
{
    public static void HandleModels(YamlSequenceNode yamlSequenceNode, List<Model> models)
    {
        foreach (var child in yamlSequenceNode.Children)
        {
            YamlParser.ThrowIfNotYamlMapping(child);
            var model = new Model();
            HandleModelMapping((YamlMappingNode)child, model);
            models.Add(model);
        }
    }

    private static void HandleModelMapping(YamlMappingNode yamlMappingNode, Model model)
    {
        foreach (var child in yamlMappingNode!.Children)
        {
            var key = YamlParser.GetScalarValue(child.Key);

            switch (key)
            {
                case "name":
                    model.Name = YamlParser.GetScalarValue(key, child.Value).ToLowerInvariant();
                    break;
                case "type":
                    model.Type = YamlParser.GetScalarValue(key, child.Value).ToLowerInvariant();
                    break;
                default:
                    throw new DDDYamlException(child.Key.Start, $"Unrecognized Key: {key}");
            }
        }
    }
}