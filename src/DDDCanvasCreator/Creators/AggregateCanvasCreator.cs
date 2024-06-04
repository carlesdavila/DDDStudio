using System.Drawing;
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
        // Get the rectangle using its ID
        var rect = svgDocument.GetElementById<SvgRectangle>("handledCommandsRect");
        if (rect == null) throw new Exception("Rectangle with ID 'handledCommandsRect' not found in the SVG.");

        // Define the properties of the rectangle
        float rectX = rect.X;
        float rectY = rect.Y;
        float rectWidth = rect.Width;
        float rectHeight = rect.Height;

        // Define the initial offset and the spacing between boxes
        var boxSize = 62.5f; // Size of the square box (25% larger than 50)
        var initialY = rectY + 60; // A little top margin to start below the existing text
        var spacingX = boxSize + 10; // Spacing between each box horizontally

        // Calculate the initial X to center the boxes horizontally within the rectangle
        var totalBoxesWidth = aggregateHandledCommands.Count * boxSize + (aggregateHandledCommands.Count - 1) * 10;
        var initialX = rectX + (rectWidth - totalBoxesWidth) / 2;

        // Text font and style for handled commands
        var fontFamily = "IBM Plex Sans";
        float fontSize = 12; // Font size
        var textColor = "#323D4F";
        var boxFillColor = "#40C7EA";

        // Get the existing group using its ID
        var handledCommandsGroup = svgDocument.GetElementById<SvgGroup>("handledCommandsGroup");
        if (handledCommandsGroup == null)
            throw new Exception("Group with ID 'handledCommandsGroup' not found in the SVG.");

        // Keep the existing handled commands text element
        var handledCommandsText = svgDocument.GetElementById<SvgText>("handledCommandsText");

        // Counter for horizontal positioning
        var index = 0;

        foreach (var command in aggregateHandledCommands)
        {
            // Create the square background for each command
            var rectElement = new SvgRectangle
            {
                X = initialX + index * spacingX, // Adjust to place boxes horizontally
                Y = initialY - 20, // Adjust to position the text inside the box
                Width = boxSize, // Square width
                Height = boxSize, // Square height
                Fill = new SvgColourServer(ColorTranslator.FromHtml(boxFillColor)),
                Stroke = SvgPaintServer.None
            };

            // Create the <text> element with the necessary properties
            var textElement = new SvgText(command)
            {
                X = new SvgUnitCollection
                    { initialX + index * spacingX + 5 }, // Adjust to center the text within the box
                Y = new SvgUnitCollection { initialY },
                Fill = new SvgColourServer(ColorTranslator.FromHtml(textColor)),
                FontFamily = fontFamily,
                FontSize = fontSize,
                FontWeight = SvgFontWeight.Bold
            };

            // Create a group to hold both the rectangle and the text
            var commandGroup = new SvgGroup();
            commandGroup.Children.Add(rectElement);
            commandGroup.Children.Add(textElement);

            // Add the group to the existing <g> group
            handledCommandsGroup.Children.Add(commandGroup);

            // Increment the counter for the next horizontal position
            index++;
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