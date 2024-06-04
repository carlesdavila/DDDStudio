using System.Text;
using DDDCanvasCreator.Models.AggregateCanvas;
using DDDCanvasCreator.Services;
using Svg;

namespace DDDCanvasCreator.Creators;

public class AggregateCanvasCreator : IYamlProcessor
{
    public void ProcessYamlAndGenerateSvg(string yamlContent, string outputFilePath)
    {
        var aggregate = ParseYaml(yamlContent);
        GenerateAggregateSvg(aggregate, outputFilePath);
    }

    private Aggregate ParseYaml(string yamlContent)
    {
        using var parser = new YamlParser(yamlContent);
        var actual = parser.ParseAggregate();

        return actual;
    }

    private void GenerateAggregateSvg(Aggregate aggregate, string outputFilePath)
    {
        // Construct the full path to the SVG template file
        var templateFilePath = Path.Combine("Templates", "aggregate-template.svg");

        // Load the SVG file as an initial template
        var svgDocument = SvgDocument.Open(templateFilePath);

        GenerateHandledCommands(aggregate.HandledCommands, svgDocument);

        GenerateNameAndDescription(aggregate, svgDocument);
        // GenerateStateTransitions(aggregate.StateTransitions, svgDocument);
        // GenerateEnforcedInvariants(aggregate.EnforcedInvariants,svgDocument);
        // GenerateCorrectivePolicies(aggregate.CorrectivePolicies, svgDocument);


        // Save the modified SVG document to the specified output file path
        svgDocument.Write(outputFilePath);
    }

    private void GenerateHandledCommands(List<string> aggregateHandledCommands, SvgDocument svgDocument)
    {
        // Get the group that contains the handled commands elements
        var cardsHcGroup = svgDocument.GetElementById<SvgGroup>("cardsHc");

        if (cardsHcGroup != null)
        {
            // Get all handled commands rectangles and texts
            var rects = cardsHcGroup.Children.OfType<SvgRectangle>().ToList();
            var texts = cardsHcGroup.Children.OfType<SvgText>().ToList();

            // Remove unused rectangles and texts
            for (var i = aggregateHandledCommands.Count; i < rects.Count; i++)
            {
                var rectId = $"rectHc{i + 1}";
                var textId = $"txtHc{i + 1}";

                var rect = rects.FirstOrDefault(r => r.ID == rectId);
                if (rect != null) cardsHcGroup.Children.Remove(rect);

                var text = texts.FirstOrDefault(t => t.ID == textId);
                if (text != null) cardsHcGroup.Children.Remove(text);
            }

            // Update existing texts with the new aggregateHandledCommands list
            for (var i = 0; i < aggregateHandledCommands.Count; i++)
            {
                var command = aggregateHandledCommands[i];
                var textId = $"txtHc{i + 1}";

                // Find and update the corresponding text
                var text = texts.FirstOrDefault(t => t.ID == textId);
                if (text != null) text.Text = command;
            }
        }
    }

    private void GenerateCorrectivePolicies(List<string> aggregateCorrectivePolicies, SvgDocument svgDocument)
    {
        throw new NotImplementedException();
    }

    private void GenerateEnforcedInvariants(List<string> aggregateEnforcedInvariants, SvgDocument svgDocument)
    {
        throw new NotImplementedException();
    }

    private void GenerateStateTransitions(List<StateTransition> aggregateStateTransitions, SvgDocument svgDocument)
    {
        throw new NotImplementedException();
    }

    private void GenerateNameAndDescription(Aggregate aggregate, SvgDocument svgDocument)
    {
        // Modify the text for the name
        var nameText = svgDocument.GetElementById<SvgText>("nameText");
        if (nameText != null) nameText.Text = $"1. Name: {aggregate.Name}";

        // Modify the text for the description
        var descriptionText = svgDocument.GetElementById<SvgText>("descriptionText");
        if (descriptionText != null)
        {
            // Clear existing text
            descriptionText.Children.Clear();

            // Assume font size is 16px
            var fontSize = 16f;
            var averageCharWidth = fontSize * 0.6f; // Average character width estimate
            var maxLineWidth = 350f; // Width of the rect in the template

            // Split the description into lines
            var lines = SplitTextIntoLines(aggregate.Description, averageCharWidth, maxLineWidth);

            // Add each line as a tspan with dy attribute
            var initialY = 181f;
            var lineHeight = fontSize * 1.2f; // 1.2em line height

            foreach (var line in lines)
            {
                var tspan = new SvgTextSpan
                {
                    X = descriptionText.X,
                    Dy = new SvgUnitCollection { new(SvgUnitType.Em, 1.2f) }, // Espacio entre líneas
                    FontSize = new SvgUnit(fontSize),
                    Fill = descriptionText.Fill,
                    Text = line
                };
                descriptionText.Children.Add(tspan);
                initialY += lineHeight;
            }
        }
    }

    private List<string> SplitTextIntoLines(string text, float charWidth, float maxLineWidth)
    {
        var words = text.Split(' ');
        var lines = new List<string>();
        var currentLine = new StringBuilder();
        var currentLineWidth = 0f;

        foreach (var word in words)
        {
            var wordWidth = word.Length * charWidth;
            if (currentLineWidth + wordWidth < maxLineWidth)
            {
                currentLine.Append(word + " ");
                currentLineWidth += wordWidth + charWidth; // Add a space width
            }
            else
            {
                lines.Add(currentLine.ToString().Trim());
                currentLine.Clear();
                currentLine.Append(word + " ");
                currentLineWidth = wordWidth + charWidth; // Reset line width with the new word
            }
        }

        if (currentLine.Length > 0) lines.Add(currentLine.ToString().Trim());

        return lines;
    }
}