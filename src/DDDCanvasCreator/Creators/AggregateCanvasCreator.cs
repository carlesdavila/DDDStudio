using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using DDDCanvasCreator.Models;
using DDDCanvasCreator.Models.AggregateCanvas;
using DDDCanvasCreator.Services;
using Svg;

namespace DDDCanvasCreator.Creators;

public class AggregateCanvasCreator : IYamlProcessor
{
    public void ProcessYamlAndGenerateSvg(string yamlContent, string outputFilePath, DddConfig config)
    {
        var aggregate = ParseYaml(yamlContent);
        GenerateAggregateSvg(aggregate, outputFilePath, config);
    }

    private Aggregate ParseYaml(string yamlContent)
    {
        using var parser = new YamlParser(yamlContent);
        var actual = parser.ParseAggregate();

        return actual;
    }

    private void GenerateAggregateSvg(Aggregate aggregate, string outputFilePath, DddConfig config)
    {
        // Process the SVG content using Svg.NET
        var svgDocument = TemplateService.GetAggregateSvgDocument();

        GenerateNameAndDescription(aggregate, svgDocument);
        GenerateEnforcedInvariants(aggregate.EnforcedInvariants, svgDocument);
        GenerateHandledCommands(aggregate.HandledCommands, svgDocument, config);
        GenerateCreatedEvents(aggregate.CreatedEvents, svgDocument, config);
        GenerateCorrectivePolicies(aggregate.CorrectivePolicies, svgDocument);
        GenerateStateTransitions(aggregate.StateTransitions, svgDocument, config);

        // Save the modified SVG document to the specified output file path
        svgDocument.Write(outputFilePath);
    }

    private void GenerateCorrectivePolicies(List<string> aggregateCorrectivePolicies, SvgDocument svgDocument)
    {
        AddHtmlBulletPoints(aggregateCorrectivePolicies, svgDocument, "correctivePoliciesFo");

        AddBulletPoints(aggregateCorrectivePolicies, svgDocument, "cpText");
    }

    private void GenerateEnforcedInvariants(List<string> aggregateEnforcedInvariants, SvgDocument svgDocument)
    {
        AddHtmlBulletPoints(aggregateEnforcedInvariants, svgDocument, "enforcedInvariantsFo");

        AddBulletPoints(aggregateEnforcedInvariants, svgDocument, "eiText");
    }

    private void AddHtmlBulletPoints(List<string> items, SvgDocument svgDocument, string foreignObjectElementId)
    {
        // Get the foreignObject element with the specified ID
        var foreignObjectElement = svgDocument.GetElementById<SvgForeignObject>(foreignObjectElementId);

        if (foreignObjectElement != null)
        {
            // Clear any existing content
            foreignObjectElement.Nodes.Clear();

            // Create the <ul> element
            var ulElement = new NonSvgElement("ul", "http://www.w3.org/1999/xhtml");
            ulElement.CustomAttributes.Add("class", "list");

            // Add each item as a <li> element
            foreach (var item in items)
            {
                var liElement = new NonSvgElement("li", "http://www.w3.org/1999/xhtml")
                {
                    Content = item
                };
                ulElement.Children.Add(liElement);
            }

            // Add the <ul> element to the foreignObject
            foreignObjectElement.Children.Add(ulElement);
        }
        else
        {
            throw new InvalidOperationException(
                $"The '{foreignObjectElementId}' element was not found in the SVG document.");
        }
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

    private void GenerateHandledCommands(List<string> aggregateHandledCommands, SvgDocument svgDocument,
        DddConfig config)
    {
        GenerateElements(aggregateHandledCommands, svgDocument, "cardsHc", "rectHc", "txtHc", "swHc", "foHc", config.HandledCommandsColor);
    }

    private void GenerateCreatedEvents(List<string> aggregateCreatedEvents, SvgDocument svgDocument, DddConfig config)
    {
        GenerateElements(aggregateCreatedEvents, svgDocument, "cardsCe", "rectCe", "txtCe", "swCe", "foCe", config.CreatedEventsColor);
    }

    private void GenerateElements(List<string> aggregateElements, SvgDocument svgDocument, string groupIdPrefix,
        string rectIdPrefix, string textIdPrefix, string switchIdPrefix, string foreignObjectIdPrefix, string color)
    {
        // Get the group that contains the elements
        var group = svgDocument.GetElementById<SvgGroup>(groupIdPrefix);

        if (group != null)
        {
            // Get all rectangles and switches
            var rects = group.Children.OfType<SvgRectangle>().ToList();
            var switches = group.Children.OfType<SvgSwitch>().ToList();

            // Remove unused rectangles and switches
            for (var i = aggregateElements.Count; i < rects.Count; i++)
            {
                var rectId = $"{rectIdPrefix}{i + 1}";
                var switchId = $"{switchIdPrefix}{i + 1}";

                var rect = rects.FirstOrDefault(r => r.ID == rectId);
                if (rect != null) group.Children.Remove(rect);

                var svgSwitch = switches.FirstOrDefault(s => s.ID == switchId);
                if (svgSwitch != null) group.Children.Remove(svgSwitch);
            }

            // Update existing texts and foreignObjects within switches with the new aggregateElements list
            for (var i = 0; i < aggregateElements.Count; i++)
            {
                var element = aggregateElements[i];
                var switchId = $"{switchIdPrefix}{i + 1}";
                var foreignObjectId = $"{foreignObjectIdPrefix}{i + 1}";
                var textId = $"{textIdPrefix}{i + 1}";

                // Find the corresponding switch
                var svgSwitch = switches.FirstOrDefault(s => s.ID == switchId);
                if (svgSwitch != null)
                {
                    // Find the corresponding foreignObject within the switch
                    var foreignObject = svgSwitch.Children.OfType<SvgForeignObject>()
                        .FirstOrDefault(f => f.ID == foreignObjectId);
                    if (foreignObject != null)
                    {
                        foreignObject.Content = null; // Clear previous content

                        // Assign the new content directly to the foreignObject
                        var xhtmlDiv = new NonSvgElement("div", "http://www.w3.org/1999/xhtml")
                        {
                            Content = FormatElement(element)
                        };
                        xhtmlDiv.CustomAttributes.Add("class", "text-card");
                        foreignObject.Nodes.Add(xhtmlDiv);
                    }

                    // Find and update the corresponding text within the switch
                    var text = svgSwitch.Children.OfType<SvgText>().FirstOrDefault(t => t.ID == textId);
                    if (text != null) text.Text = element;
                    
                    // Update the color of the rectangle
                    var rect = rects.FirstOrDefault(r => r.ID == $"{rectIdPrefix}{i + 1}");
                    if (rect != null) rect.Fill = new SvgColourServer(ColorTranslator.FromHtml(color));
                }
            }
        }
    }

    private void GenerateStateTransitions(List<StateTransition> aggregateStateTransitions, SvgDocument svgDocument,
        DddConfig config)
    {
        // Size and position of the main rectangle in the SVG
        const float rectX = 370;
        const float rectY = 91;
        const float rectWidth = 680;
        float rectHeight = 230;

        // Margin inside the rectangle for elements
        const float marginX = 20;
        const float marginY = 20;

        // Fixed size of each state
        const float stateWidth = 100;
        const float stateHeight = 50;

        // Vertical distance between levels
        const float levelSpacingY = 60;
        // Horizontal distance between states
        const float stateSpacingX = 120;

        // Dictionary to store the position of each state
        var statePositions = new Dictionary<string, (float x, float y)>();

        // Queue to manage the distribution of states
        var stateQueue = new Queue<StateTransition>(aggregateStateTransitions);

        // List of colors to alternate
        var colors = config.AggregateStatesColors.Select(ColorTranslator.FromHtml).ToList();

        // Color iterator
        var colorIndex = 0;

        // Variables to control the position of the states
        var currentStateX = rectX + marginX;
        var currentStateY = rectY + marginY;
        var currentLevel = 0;

        // Dictionary to avoid processing a state more than once
        var processedStates = new HashSet<string>();

        // Get the stateGroup from the SVG document
        var stateGroup = svgDocument.GetElementById<SvgGroup>("stateGroup");

        while (stateQueue.Count > 0)
        {
            var stateTransition = stateQueue.Dequeue();

            if (!processedStates.Contains(stateTransition.State))
            {
                // Place the current state
                statePositions[stateTransition.State] = (currentStateX, currentStateY);

                // Create the rectangle to represent the state
                var stateRect = new SvgRectangle
                {
                    X = new SvgUnit(currentStateX),
                    Y = new SvgUnit(currentStateY),
                    Width = new SvgUnit(stateWidth),
                    Height = new SvgUnit(stateHeight),
                    Fill = new SvgColourServer(colors[colorIndex]),
                    Filter = new Uri("url(#dropShadow)", UriKind.Relative)
                };
                stateRect.CustomAttributes.Add("class", "state-card");
                stateGroup.Children.Add(stateRect);

                // Alternate the color
                colorIndex = (colorIndex + 1) % colors.Count;

                // Add the state name
                var stateText = new SvgText
                {
                    X = [new SvgUnit(currentStateX + stateWidth / 2)],
                    Y = [new SvgUnit(currentStateY + stateHeight / 2 + 5)], // Adjustment to vertically center
                    FontSize = new SvgUnit(12),
                    FontWeight = SvgFontWeight.Bold,
                    Fill = new SvgColourServer(Color.Black),
                    TextAnchor = SvgTextAnchor.Middle,
                    ID = "stateText_" + Guid.NewGuid().ToString("N")
                };
                stateText.Text = stateTransition.State;
                stateGroup.Children.Add(stateText);

                // Mark the state as processed
                processedStates.Add(stateTransition.State);

                // Add transition states to the queue
                foreach (var transition in stateTransition.Transitions)
                    if (!processedStates.Contains(transition.To))
                        stateQueue.Enqueue(aggregateStateTransitions.First(t => t.State == transition.To));

                // Update the x position for the next state
                currentStateX += stateWidth + stateSpacingX;

                // If the end of the rectangle is reached, move down to the next level
                if (currentStateX + stateWidth + marginX > rectX + rectWidth)
                {
                    currentStateX = rectX + marginX;
                    currentStateY += stateHeight + levelSpacingY;
                    currentLevel++;
                }
            }
        }

        // Draw transition arrows
        foreach (var stateTransition in aggregateStateTransitions)
        {
            var (startX, startY) = statePositions[stateTransition.State];

            foreach (var transition in stateTransition.Transitions)
                if (statePositions.TryGetValue(transition.To, out var endPosition))
                {
                    var (endX, endY) = endPosition;

                    // Calculate the start and end points at the edges of the rectangles
                    var arrowStartX = startX + stateWidth / 2;
                    var arrowStartY = startY + stateHeight / 2;
                    var arrowEndX = endX + stateWidth / 2;
                    var arrowEndY = endY + stateHeight / 2;

                    // Adjust the arrow position based on the relationship between states
                    if (startX < endX)
                    {
                        arrowStartX = startX + stateWidth;
                        arrowEndX = endX;
                    }
                    else if (startX > endX)
                    {
                        arrowStartX = startX;
                        arrowEndX = endX + stateWidth;
                    }

                    if (startY < endY)
                    {
                        arrowStartY = startY + stateHeight;
                        arrowEndY = endY;
                    }
                    else if (startY > endY)
                    {
                        arrowStartY = startY;
                        arrowEndY = endY + stateHeight;
                    }

                    // Draw the arrow line
                    var arrowLine = new SvgLine
                    {
                        StartX = new SvgUnit(arrowStartX),
                        StartY = new SvgUnit(arrowStartY),
                        EndX = new SvgUnit(arrowEndX),
                        EndY = new SvgUnit(arrowEndY),
                        Stroke = new SvgColourServer(Color.Black),
                        StrokeWidth = new SvgUnit(1),
                        MarkerEnd = new Uri("url(#arrowhead)", UriKind.Relative)
                    };
                    stateGroup.Children.Add(arrowLine);
                }
        }
    }

    private void GenerateNameAndDescription(Aggregate aggregate, SvgDocument svgDocument)
    {
        // Modify the text for the name
        var nameText = svgDocument.GetElementById<SvgText>("nameText");
        if (nameText != null) nameText.Text = $"1. Name: {aggregate.Name}";

        // Modify the text for the description in the div within the foreignObject
        var descriptionFo = svgDocument.GetElementById<SvgForeignObject>("descriptionFo");
        if (descriptionFo != null)
        {
            // Clear existing content
            descriptionFo.Nodes.Clear();

            // Assign the new content directly to the foreignObject
            var nonSvgElement = new NonSvgElement("div", "http://www.w3.org/1999/xhtml")
            {
                Content = aggregate.Description
            };
            nonSvgElement.CustomAttributes.Add("class", "description");
            descriptionFo.Children.Add(nonSvgElement);
        }

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

    private string FormatElement(string element)
    {
        if (string.IsNullOrEmpty(element))
            return element;

        // Convert camelCase or PascalCase to words separated by spaces
        var formattedElement = Regex.Replace(element, "([a-z])([A-Z])", "$1 $2");

        // Convert kebab-case to words separated by spaces
        formattedElement = formattedElement.Replace("-", " ");

        // Ensure the first letter is capitalized
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(formattedElement.ToLower());
    }
}