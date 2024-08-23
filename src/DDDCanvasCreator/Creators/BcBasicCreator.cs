using System.Drawing;
using DDDCanvasCreator.Models;
using DDDCanvasCreator.Models.BoundedContextBasic;
using DDDCanvasCreator.Services;
using Svg;

namespace DDDCanvasCreator.Creators;

public class BcBasicCreator : IYamlProcessor
{
    public void ProcessYamlAndGenerateSvg(string yamlContent, string outputFilePath, DddConfig config)
    {
        var boundedContexts = ParseYaml(yamlContent);
        GenerateBoundedContextSvg(boundedContexts.BoundedContexts, outputFilePath, config);
    }

    private BoundedContextsBasic ParseYaml(string yamlContent)
    {
        using var parser = new YamlParser(yamlContent);
        var actual = parser.ParseBoundedContextsBasic();
        return actual;
    }

    private void GenerateBoundedContextSvg(List<BoundedContext> contexts, string outputFilePath, DddConfig config)
    {
        const int margin = 20;
        const int contextWidth = 460;
        const int contextHeight = 460;

        // Load the base SVG document from TemplateService
        var svgDoc = TemplateService.GetContextSvgDocument();

        var colors = config.BoundedContextColors; // Predefined colors
        var colorIndex = 0;

        var x = margin;
        var y = margin;

        foreach (var context in contexts)
        {
            var contextColor = colors[colorIndex];
            colorIndex = (colorIndex + 1) % colors.Count;

            DrawContext(svgDoc, context, contextColor, x, y, margin);

            x += contextWidth + margin;
            if (x + contextWidth > svgDoc.Width - margin)
            {
                x = margin;
                y += contextHeight + margin;
            }
        }

        // Save the modified SVG document to the specified output file path
        svgDoc.Write(outputFilePath);
    }

    private void DrawContext(SvgDocument svgDoc, BoundedContext context, string contextColor, int x, int y, int margin)
    {
        const int contextWidth = 460;
        const int contextHeight = 460;
        const int borderRadius = 20;
        const int strokeWidth = 5;
        const int titleHeight = 40;

        // Create and configure the context rectangle
        var contextRect = new SvgRectangle
        {
            X = x,
            Y = y + titleHeight,
            Width = contextWidth,
            Height = contextHeight,
            CornerRadiusX = borderRadius,
            CornerRadiusY = borderRadius,
            Stroke = new SvgColourServer(ColorTranslator.FromHtml(contextColor)),
            StrokeWidth = strokeWidth,
            StrokeDashArray = new SvgUnitCollection { 4 }
        };
        contextRect.CustomAttributes.Add("class", "context");
        svgDoc.Children.Add(contextRect);

        // Create and configure the context title
        var title = new SvgText(context.Name!.ToUpper())
        {
            X = [x + contextWidth / 2],
            Y = [y + titleHeight / 2],
            Fill = new SvgColourServer(ColorTranslator.FromHtml(contextColor)),
            TextAnchor = SvgTextAnchor.Middle,
            FontSize = 28,
            FontWeight = SvgFontWeight.Bold
        };
        title.CustomAttributes.Add("class", "context-text");
        svgDoc.Children.Add(title);

        DrawModels(svgDoc, context.Models, x, y + titleHeight + margin, margin);
    }

    private void DrawModels(SvgDocument svgDoc, List<Model> models, int x, int y, int margin)
    {
        const int modelWidth = 170;
        const int modelHeight = 80;
        const int borderRadius = 15;
        const int titleHeight = 40;

        // Positioning for the first model (Core Concept)
        var coreModel = models.FirstOrDefault(m => m.Type == "CoreConcept");
        if (coreModel != null)
        {
            var coreModelRect = new SvgRectangle
            {
                X = x + 160,
                Y = y + 20,
                Width = 180,
                Height = 120,
                CornerRadiusX = borderRadius,
                CornerRadiusY = borderRadius,
                Stroke = new SvgColourServer(ColorTranslator.FromHtml("#00CC66")),
                StrokeWidth = 5
            };
            coreModelRect.CustomAttributes.Add("class", "model-core");
            svgDoc.Children.Add(coreModelRect);

            var coreModelName = new SvgText(coreModel.Name)
            {
                X = new SvgUnitCollection { x + 250 },
                Y = new SvgUnitCollection { y + 80 },
                TextAnchor = SvgTextAnchor.Middle,
                FontSize = 20,
                FontWeight = SvgFontWeight.Bold
            };
            coreModelName.CustomAttributes.Add("class", "text-main");
            svgDoc.Children.Add(coreModelName);
        }

        // Positioning for the sub models
        var subModels = models.Where(m => m.Type != "CoreConcept").ToList();
        var positions = new List<(int x, int y)>
        {
            (50, 220), (280, 220), (50, 310), (280, 310), (50, 400), (280, 400)
        };

        for (var i = 0; i < subModels.Count && i < positions.Count; i++)
        {
            var subModel = subModels[i];
            var (posX, posY) = positions[i];

            var subModelRect = new SvgRectangle
            {
                X = x + posX,
                Y = y + posY,
                Width = modelWidth,
                Height = modelHeight,
                CornerRadiusX = borderRadius,
                CornerRadiusY = borderRadius,
                Stroke = new SvgColourServer(ColorTranslator.FromHtml("#00a2ff")),
                StrokeWidth = 5
            };
            subModelRect.CustomAttributes.Add("class", "model-sub");
            svgDoc.Children.Add(subModelRect);

            var subModelName = new SvgText(subModel.Name)
            {
                X = new SvgUnitCollection { x + posX + modelWidth / 2 },
                Y = new SvgUnitCollection { y + posY + modelHeight / 2 },
                TextAnchor = SvgTextAnchor.Middle,
                FontSize = 20,
                FontWeight = SvgFontWeight.Bold
            };
            subModelName.CustomAttributes.Add("class", "text-main");
            svgDoc.Children.Add(subModelName);
        }
    }
}