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
        var contextWidth = config.BoundedContextWidth;

        var svgDoc = TemplateService.GetContextSvgDocument();

        // Calculate the width and height of the SVG document
        var svgWidth = contexts.Count * (contextWidth + margin) + margin;
        var svgHeight = CalculateSvgHeight(contexts, margin);
        svgDoc.Width = new SvgUnit(svgWidth);
        svgDoc.Height = new SvgUnit(svgHeight); 
        svgDoc.ViewBox = new SvgViewBox(0, 0, svgWidth, svgHeight);


        var colors = config.BoundedContextColors;
        var colorIndex = 0;

        var x = margin;
        var y = margin;

        foreach (var context in contexts)
        {
            var contextColor = colors[colorIndex];
            colorIndex = (colorIndex + 1) % colors.Count;

            // Separate core models and sub models
            var coreModelsCount = context.Models.Count(m => m.Type == "CoreConcept");
            var subModelsCount = context.Models.Count(m => m.Type != "CoreConcept");

            // Calculate the number of rows needed for core models and sub models
            var coreModelRows = (int)Math.Ceiling(coreModelsCount / 2.0);
            var subModelRows = (int)Math.Ceiling(subModelsCount / 2.0);

            // Calculate the height for core models and sub models
            var coreModelsHeight = coreModelRows * 90; // 90 is the height of each model with margin
            var subModelsHeight = subModelRows * 90; // 90 is the height of each model with margin

            // Total context height includes core models, sub models, title, and extra margin
            var contextHeight = coreModelsHeight + subModelsHeight + 100; // 100 is for title and extra margin

            DrawContext(svgDoc, context, contextColor, x, y, margin, contextWidth, contextHeight);

            x += contextWidth + margin;
            if (x + contextWidth > svgWidth - margin)
            {
                x = margin;
                y += contextHeight + margin;
            }
        }

        // Save the modified SVG document to the specified output file path
        svgDoc.Write(outputFilePath);
    }

    private void DrawContext(SvgDocument svgDoc, BoundedContext context, string contextColor, int x, int y, int margin,
        int contextWidth, int contextHeight)
    {
        const int borderRadius = 20;
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
            Stroke = new SvgColourServer(ColorTranslator.FromHtml(contextColor))
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

        DrawModels(svgDoc, context.Models, x, y + titleHeight + margin, margin, contextWidth);
    }

    private void DrawModels(SvgDocument svgDoc, List<Model> models, int x, int y, int margin, int contextWidth)
    {
        var modelWidth = (contextWidth - 3 * margin) / 2; // Two models with margin between and on sides
        var modelHeight = modelWidth / 2;
        const int borderRadius = 15;

        // Separate core models and sub models
        var coreModels = models.Where(m => m.Type == "CoreConcept").ToList();
        var subModels = models.Where(m => m.Type != "CoreConcept").ToList();

        // Positioning for the core models
        for (var i = 0; i < coreModels.Count; i++)
        {
            var model = coreModels[i];
            var posX = x + (coreModels.Count == 1
                ? (contextWidth - modelWidth) / 2
                : margin + i % 2 * (modelWidth + margin));

            var modelRect = new SvgRectangle
            {
                X = posX,
                Y = y,
                Width = modelWidth,
                Height = modelHeight,
                CornerRadiusX = borderRadius,
                CornerRadiusY = borderRadius
            };
            modelRect.CustomAttributes.Add("class", "model-core");
            svgDoc.Children.Add(modelRect);

            var modelName = new SvgText(model.Name)
            {
                X = [posX + modelWidth / 2],
                Y = [y + modelHeight / 2],
                TextAnchor = SvgTextAnchor.Middle
            };
            modelName.CustomAttributes.Add("class", "model-text");
            svgDoc.Children.Add(modelName);
        }

        // Positioning for the sub models
        var subModelStartY = y + (coreModels.Count > 0 ? modelHeight + margin : 0);
        for (var i = 0; i < subModels.Count; i++)
        {
            var model = subModels[i];
            var posX = x + margin + i % 2 * (modelWidth + margin);
            var posY = subModelStartY + i / 2 * (modelHeight + margin);

            var modelRect = new SvgRectangle
            {
                X = posX,
                Y = posY,
                Width = modelWidth,
                Height = modelHeight,
                CornerRadiusX = borderRadius,
                CornerRadiusY = borderRadius
            };
            modelRect.CustomAttributes.Add("class", "model-sub");
            svgDoc.Children.Add(modelRect);

            var modelName = new SvgText(model.Name)
            {
                X = [posX + modelWidth / 2],
                Y = [posY + modelHeight / 2],
                TextAnchor = SvgTextAnchor.Middle
            };
            modelName.CustomAttributes.Add("class", "model-text");
            svgDoc.Children.Add(modelName);
        }
    }

    private int CalculateSvgHeight(List<BoundedContext> contexts, int margin)
    {
        var totalHeight = margin;
        foreach (var context in contexts)
        {
            // Separate core models and sub models
            var coreModelsCount = context.Models.Count(m => m.Type == "CoreConcept");
            var subModelsCount = context.Models.Count(m => m.Type != "CoreConcept");

            // Calculate the number of rows needed for core models and sub models
            var coreModelRows = (int)Math.Ceiling(coreModelsCount / 2.0);
            var subModelRows = (int)Math.Ceiling(subModelsCount / 2.0);

            // Calculate the height for core models and sub models
            var coreModelsHeight = coreModelRows * 90; // 90 is the height of each model with margin
            var subModelsHeight = subModelRows * 90; // 90 is the height of each model with margin

            // Total context height includes core models, sub models, title, and extra margin
            var contextHeight = coreModelsHeight + subModelsHeight + 100; // 100 is for title and extra margin
            totalHeight += contextHeight + margin;
        }

        return totalHeight;
    }
}