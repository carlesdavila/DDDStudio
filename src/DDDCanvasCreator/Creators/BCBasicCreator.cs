using System.Drawing;
using Svg;

namespace DDDCanvasCreator.Creators;

public class BcBasicCreator
{
    public void CreateSvg(List<BoundedContext> contexts,  string outputPath)
    {
        const int width = 800;
        const int height = 600;
        const int contextWidth = 350;
        const int contextHeight = 200;
        const int margin = 20;
        const int modelHeight = 30;
        const int titleHeight = 20;

        var svgDoc = new SvgDocument
        {
            Width = width,
            Height = height
        };

        var x = margin;
        var y = margin;

        foreach (var context in contexts)
        {
            var contextRect = new SvgRectangle
            {
                X = x,
                Y = y,
                Width = contextWidth,
                Height = contextHeight,
                Fill = new SvgColourServer(ColorTranslator.FromHtml(context.Color)),
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 1
            };
            svgDoc.Children.Add(contextRect);

            var title = new SvgText(context.Name)
            {
                X = [x + contextWidth / 2],
                Y = [y + titleHeight],
                TextAnchor = SvgTextAnchor.Middle,
                FontSize = 16,
                Fill = new SvgColourServer(Color.Black)
            };
            svgDoc.Children.Add(title);

            var modelY = y + titleHeight + margin;
            foreach (var model in context.Models)
            {
                var modelRect = new SvgRectangle
                {
                    X = x + margin,
                    Y = modelY,
                    Width = contextWidth - 2 * margin,
                    Height = modelHeight,
                    Fill = new SvgColourServer(Color.White),
                    Stroke = new SvgColourServer(Color.Black),
                    StrokeWidth = 1
                };
                svgDoc.Children.Add(modelRect);

                var modelName = new SvgText($"{model.Name} ({model.Type})")
                {
                    X = [x + contextWidth / 2],
                    Y = [modelY + modelHeight / 2 + 5],
                    TextAnchor = SvgTextAnchor.Middle,
                    FontSize = 12,
                    Fill = new SvgColourServer(Color.Black)
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

        svgDoc.Write(outputPath);
    }
}