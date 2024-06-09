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


        GenerateNameAndDescription(aggregate, svgDocument);
        GenerateEnforcedInvariants(aggregate.EnforcedInvariants, svgDocument);
        GenerateHandledCommands(aggregate.HandledCommands, svgDocument);
        GenerateCreatedEvents(aggregate.CreatedEvents, svgDocument);
        GenerateCorrectivePolicies(aggregate.CorrectivePolicies, svgDocument);
        // GenerateStateTransitions(aggregate.StateTransitions, svgDocument);
        
        // Save the modified SVG document to the specified output file path
        svgDocument.Write(outputFilePath);
    }

    private void GenerateCorrectivePolicies(List<string> aggregateCorrectivePolicies, SvgDocument svgDocument)
    {
        AddBulletPoints(aggregateCorrectivePolicies, svgDocument, "cpText");
    }

    private void GenerateEnforcedInvariants(List<string> aggregateEnforcedInvariants, SvgDocument svgDocument)
    {
        AddBulletPoints(aggregateEnforcedInvariants, svgDocument, "eiText");
    }

    private void AddBulletPoints(List<string> items, SvgDocument svgDocument, string textElementId)
    {
        // Get the text element with the specified ID
        var textElement = svgDocument.GetElementById<SvgText>(textElementId);

        if (textElement != null)
        {
            // Clear any existing content
            textElement.Children.Clear();

            // Add each item as a new tspan
            foreach (var item in items)
            {
                var tspan = new SvgTextSpan
                {
                    X = [19], // Set the x position
                    Dy = [new SvgUnit(SvgUnitType.Em, 1.2f)],
                    Text = "• " + item // Add the bullet point and text
                };
                textElement.Children.Add(tspan);
            }
        }
        else
        {
            throw new InvalidOperationException($"The '{textElementId}' element was not found in the SVG document.");
        }
    }

    private void GenerateHandledCommands(List<string> aggregateHandledCommands, SvgDocument svgDocument)
    {
        GenerateElements(aggregateHandledCommands, svgDocument, "cardsHc", "rectHc", "txtHc");
    }

    private void GenerateCreatedEvents(List<string> aggregateCreatedEvents, SvgDocument svgDocument)
    {
        GenerateElements(aggregateCreatedEvents, svgDocument, "cardsCe", "rectCe", "txtCe");
    }

    private void GenerateElements(List<string> aggregateElements, SvgDocument svgDocument, string groupIdPrefix,
        string rectIdPrefix, string textIdPrefix)
    {
        // Get the group that contains the elements
        var group = svgDocument.GetElementById<SvgGroup>(groupIdPrefix);

        if (group != null)
        {
            // Get all rectangles and texts
            var rects = group.Children.OfType<SvgRectangle>().ToList();
            var texts = group.Children.OfType<SvgText>().ToList();

            // Remove unused rectangles and texts
            for (var i = aggregateElements.Count; i < rects.Count; i++)
            {
                var rectId = $"{rectIdPrefix}{i + 1}";
                var textId = $"{textIdPrefix}{i + 1}";

                var rect = rects.FirstOrDefault(r => r.ID == rectId);
                if (rect != null) group.Children.Remove(rect);

                var text = texts.FirstOrDefault(t => t.ID == textId);
                if (text != null) group.Children.Remove(text);
            }

            // Update existing texts with the new aggregateElements list
            for (var i = 0; i < aggregateElements.Count; i++)
            {
                var element = aggregateElements[i];
                var textId = $"{textIdPrefix}{i + 1}";

                // Find and update the corresponding text
                var text = texts.FirstOrDefault(t => t.ID == textId);
                if (text != null) text.Text = element;
            }
        }
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