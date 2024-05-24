using System.Drawing;
using DDDCanvasCreator.Models.BoundedContextBasic;
using DDDCanvasCreator.Services;
using Svg;
using YamlDotNet.Serialization;

namespace DDDCanvasCreator.Creators;

public class BcBasicCreator : IYamlProcessor
{
    public void ProcessYamlAndGenerateSvg(string yamlContent, string outputFilePath)
    {
        var boundedContexts = ParseYaml(yamlContent);
        GenerateBoundedContextSvg(boundedContexts, outputFilePath);
    }

    private List<BoundedContext> ParseYaml(string yamlContent)
    {
        var deserializer = new DeserializerBuilder().Build();
        var result = deserializer.Deserialize<Dictionary<string, List<BoundedContext>>>(yamlContent);
        return result["bounded_contexts"];
    }

    private void GenerateBoundedContextSvg(List<BoundedContext> contexts, string outputFilePath)
    {
        const int width = 800;
        const int height = 600;
        const int contextWidth = 350;
        const int contextHeight = 200;
        const int margin = 20;
        const int modelHeight = 30;
        const int titleHeight = 25;
        const int modelTitleHeight = 20;
        const int strokeWidth = 3;
        const int borderRadius = 10;

        var svgDoc = new SvgDocument
        {
            Width = width,
            Height = height
        };

        var x = margin;
        var y = margin;

        foreach (var context in contexts)
        {
            // Draw context rectangle
            var contextRect = new SvgRectangle
            {
                X = x,
                Y = y,
                Width = contextWidth,
                Height = contextHeight,
                Fill = new SvgColourServer(Color.White),
                Stroke = new SvgColourServer(ColorTranslator.FromHtml(context.Color)),
                StrokeWidth = strokeWidth,
                StrokeLineCap = SvgStrokeLineCap.Round,
                CornerRadiusX = borderRadius,
                CornerRadiusY = borderRadius
            };
            svgDoc.Children.Add(contextRect);

            // Draw context title
            var title = new SvgText(context.Name.ToUpper())
            {
                X = new SvgUnitCollection { x + contextWidth / 2 },
                Y = new SvgUnitCollection { y + titleHeight },
                TextAnchor = SvgTextAnchor.Middle,
                FontSize = 18,
                FontWeight = SvgFontWeight.Bold,
                Fill = new SvgColourServer(ColorTranslator.FromHtml(context.Color))
            };
            svgDoc.Children.Add(title);

            var modelY = y + titleHeight + margin;
            foreach (var model in context.Models)
            {
                // Draw model rectangle
                var modelRect = new SvgRectangle
                {
                    X = x + margin,
                    Y = modelY,
                    Width = contextWidth - 2 * margin,
                    Height = modelHeight,
                    Fill = new SvgColourServer(ColorTranslator.FromHtml("#14a32c")),
                    Stroke = new SvgColourServer(ColorTranslator.FromHtml("#14a32c")),
                    StrokeWidth = strokeWidth,
                    StrokeLineCap = SvgStrokeLineCap.Round,
                    CornerRadiusX = borderRadius / 2,
                    CornerRadiusY = borderRadius / 2
                };
                svgDoc.Children.Add(modelRect);

                // Draw model name
                var modelName = new SvgText(model.Name)
                {
                    X = new SvgUnitCollection { x + contextWidth / 2 },
                    Y = new SvgUnitCollection { modelY + modelTitleHeight },
                    TextAnchor = SvgTextAnchor.Middle,
                    FontSize = 14,
                    FontWeight = SvgFontWeight.Bold,
                    Fill = new SvgColourServer(Color.White)
                };
                svgDoc.Children.Add(modelName);

                modelY += modelHeight + margin / 2;
            }

            y += contextHeight + margin;
            if (y + contextHeight > height - margin)
            {
                y = margin;
                x += contextWidth + margin;
            }
        }

        svgDoc.Write(outputFilePath);
    }
}