using System.Drawing;
using DDDCanvasCreator.Extensions;
using DDDCanvasCreator.Models;
using DDDCanvasCreator.Models.Subdomains;
using DDDCanvasCreator.Services;
using Svg;

namespace DDDCanvasCreator.Creators;

public class SubdomainsCanvasCreator : IYamlProcessor
{
    public void ProcessYamlAndGenerateSvg(string yamlContent, string outputFilePath, DddConfig config)
    {
        var subdomains = ParseYaml(yamlContent);
        GenerateSubdomainsSvg(subdomains.Subdomains, outputFilePath, config);
    }


    private SubdomainCollection ParseYaml(string yamlContent)
    {
        using var parser = new YamlParser(yamlContent);
        return parser.ParseSubdomainCollection();
    }

    private void GenerateSubdomainsSvg(List<Subdomain> subdomains, string outputFilePath, DddConfig config)
    {
        const int margin = 20;
        const int maxCardsPerRow = 3; // Adjust as needed

        var (svgWidth, svgHeight) =
            CalculateSvgWidthAndHeight(subdomains.Count, config.SubdomainWidth, margin, maxCardsPerRow);

        var svgDoc = TemplateService.GetSubdomainSvgDocument();
        svgDoc.Width = svgWidth;
        svgDoc.Height = svgHeight;
        svgDoc.ViewBox = new SvgViewBox(0, 0, svgWidth, svgHeight);

        var cardWidth = config.SubdomainWidth;
        var cardHeight = (int)Math.Round(cardWidth / 1.8);
        var barHeight = (int)Math.Round((decimal)(cardHeight / 5));
        var barBottomMargin = (int)Math.Round(barHeight * 0.4); // Calculate margin as 40% of bar height
        var fontSize = cardHeight / 6;

        var x = margin;
        var y = margin;

        var colors = config.SubdomainColors;
        var colorIndex = 0;
        var cardsInRow = 0;

        foreach (var subdomain in subdomains)
        {
            var bgColor = colors[colorIndex];
            colorIndex = (colorIndex + 1) % colors.Count;

            var card = new SvgRectangle
            {
                X = x,
                Y = y,
                Width = cardWidth,
                Height = cardHeight,
                Fill = new SvgColourServer(ColorTranslator.FromHtml(bgColor)),
                CustomAttributes = { { "class", "card" } }
            };
            svgDoc.AddRectWithText(card, subdomain.Name, "text-title", fontSize);

            var bar = new SvgRectangle
            {
                X = x,
                Y = y + cardHeight - barHeight - barBottomMargin,
                Width = cardWidth,
                Height = barHeight,
                Fill = new SvgColourServer(ColorTranslator.FromHtml("#000000")),
                CustomAttributes = { { "class", "bar" } }
            };
            
            svgDoc.AddRectWithText(bar, subdomain.Type, "text-subtitle", fontSize/2);

            x += cardWidth + margin;
            cardsInRow++;

            if (cardsInRow >= maxCardsPerRow)
            {
                x = margin;
                y += cardHeight + margin;
                cardsInRow = 0;
            }
        }

        svgDoc.Write(outputFilePath);
    }

    private (int width, int height) CalculateSvgWidthAndHeight(int subdomainCount, int subdomainWidth, int margin,
        int maxCardsPerRow)
    {
        var cardHeight = (int)Math.Round(subdomainWidth / 1.8);
        var rows = (int)Math.Ceiling(subdomainCount / (double)maxCardsPerRow);
        var width = (subdomainWidth + margin) * Math.Min(subdomainCount, maxCardsPerRow) + margin;
        var height = (cardHeight + margin) * rows + margin;
        return (width, height);
    }
}