using System.Drawing;
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
        const int titleFontSize = 18;
        const int subtitleFontSize = 16;
        const int maxCardsPerRow = 3; // Adjust as needed

        var cardWidth = config.SubdomainWidth;
        var cardHeight = (int)Math.Round(cardWidth / 1.8); // Example ratio, adjust as needed
        var barHeight = (int)Math.Round((decimal)(cardHeight / 5)); // Example ratio, adjust as needed
        var svgDoc = TemplateService.GetSubdomainSvgDocument();

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
            svgDoc.Children.Add(card);

            var bar = new SvgRectangle
            {
                X = x,
                Y = y + cardHeight - barHeight,
                Width = cardWidth,
                Height = barHeight,
                Fill = new SvgColourServer(ColorTranslator.FromHtml("#000000")),
                CustomAttributes = { { "class", "bar" } }
            };
            svgDoc.Children.Add(bar);

            var title = new SvgText(subdomain.Name)
            {
                X = [new SvgUnit(x + 20)],
                Y = [new SvgUnit(y + cardHeight / 2)],
                CustomAttributes = { { "class", "text-title" } }
            };
            svgDoc.Children.Add(title);

            var subtitle = new SvgText(subdomain.Type)
            {
                X = [new SvgUnit(x + 45)],
                Y = [new SvgUnit(y + cardHeight - 5)],
                CustomAttributes = { { "class", "text-subtitle" } }
            };
            svgDoc.Children.Add(subtitle);

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
}