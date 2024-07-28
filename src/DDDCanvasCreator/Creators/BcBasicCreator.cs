using System.Drawing;
using DDDCanvasCreator.Models.BoundedContextBasic;
using DDDCanvasCreator.Services;
using Svg;

namespace DDDCanvasCreator.Creators;

public class BcBasicCreator : IYamlProcessor
{
    public void ProcessYamlAndGenerateSvg(string yamlContent, string outputFilePath)
    {
        var boundedContexts = ParseYaml(yamlContent);
        GenerateBoundedContextSvg(boundedContexts.BoundedContexts, outputFilePath);
    }

    private BoundedContextsBasic ParseYaml(string yamlContent)
    {
        using var parser = new YamlParser(yamlContent);
        var actual = parser.ParseBoundedContextsBasic();
        return actual;
    }

    private void GenerateBoundedContextSvg(List<BoundedContext> contexts, string outputFilePath)
    {
        const int margin = 20;

        // Load the base SVG document from TemplateService
        var svgDoc = TemplateService.GetContextSvgDocument();

        var colors = new List<string> { "#0000FF", "#008000", "#FF0000", "#FFFF00" }; // Predefined colors
        var colorIndex = 0;

        var x = margin;
        var y = margin;

        foreach (var context in contexts)
        {
            var contextColor = colors[colorIndex];
            colorIndex = (colorIndex + 1) % colors.Count;

            DrawContext(svgDoc, context, contextColor, x, y, margin);

            x += Constants.ContextWidth + margin;
            if (x + Constants.ContextWidth > svgDoc.Width - margin)
            {
                x = margin;
                y += Constants.ContextHeight + margin;
            }
        }

        // Save the modified SVG document to the specified output file path
        svgDoc.Write(outputFilePath);
    }

    private void DrawContext(SvgDocument svgDoc, BoundedContext context, string contextColor, int x, int y, int margin)
    {
        const int contextWidth = Constants.ContextWidth;
        const int contextHeight = Constants.ContextHeight;
        const int borderRadius = 10;
        const int strokeWidth = 3;
        const int titleHeight = 25;

        // Create and configure the context rectangle
        var contextRect = new SvgRectangle
        {
            X = x,
            Y = y,
            Width = contextWidth,
            Height = contextHeight,
            CornerRadiusX = borderRadius,
            CornerRadiusY = borderRadius,
            Stroke = new SvgColourServer(ColorTranslator.FromHtml(contextColor)),
            StrokeWidth = strokeWidth
        };
        contextRect.CustomAttributes.Add("class", "context");
        svgDoc.Children.Add(contextRect);

        // Create and configure the context title
        var title = new SvgText(context.Name!.ToUpper())
        {
            X = [x + contextWidth / 2],
            Y = [y + titleHeight],
            Fill = new SvgColourServer(ColorTranslator.FromHtml(contextColor))
        };
        title.CustomAttributes.Add("class", "context-text");
        svgDoc.Children.Add(title);

        DrawModels(svgDoc, context.Models, x, y + titleHeight + margin, contextWidth, margin);
    }

    private void DrawModels(SvgDocument svgDoc, List<Model> models, int x, int y, int contextWidth, int margin)
    {
        const int modelHeight = 30;
        const int modelTitleHeight = 20;
        const int borderRadius = 10;

        foreach (var model in models)
        {
            // Create and configure the model rectangle
            var modelRect = new SvgRectangle
            {
                X = x + margin,
                Y = y,
                Width = contextWidth - 2 * margin,
                Height = modelHeight,
                CornerRadiusX = borderRadius / 2,
                CornerRadiusY = borderRadius / 2
            };
            modelRect.CustomAttributes.Add("class", "model");
            svgDoc.Children.Add(modelRect);

            // Create and configure the model name
            var modelName = new SvgText(model.Name)
            {
                X = [x + contextWidth / 2],
                Y = [y + modelTitleHeight]
            };
            modelName.CustomAttributes.Add("class", "model-text");
            svgDoc.Children.Add(modelName);

            y += modelHeight + margin / 2;
        }
    }
}