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


        GenerateNameAndDescription(aggregate, svgDocument);
        GenerateEnforcedInvariants(aggregate.EnforcedInvariants, svgDocument);
        GenerateHandledCommands(aggregate.HandledCommands, svgDocument);
        GenerateCreatedEvents(aggregate.CreatedEvents, svgDocument);
        GenerateCorrectivePolicies(aggregate.CorrectivePolicies, svgDocument);
        GenerateStateTransitions(aggregate.StateTransitions, svgDocument);

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

    private void GenerateHandledCommands(List<string> aggregateHandledCommands, SvgDocument svgDocument)
    {
        GenerateElements(aggregateHandledCommands, svgDocument, "cardsHc", "rectHc", "txtHc", "swHc", "foHc");
    }

    private void GenerateCreatedEvents(List<string> aggregateCreatedEvents, SvgDocument svgDocument)
    {
        GenerateElements(aggregateCreatedEvents, svgDocument, "cardsCe", "rectCe", "txtCe", "swCe", "foCe");
    }

    private void GenerateElements(List<string> aggregateElements, SvgDocument svgDocument, string groupIdPrefix,
        string rectIdPrefix, string textIdPrefix, string switchIdPrefix, string foreignObjectIdPrefix)
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
                }
            }
        }
    }

    private void GenerateStateTransitions(List<StateTransition> aggregateStateTransitions, SvgDocument svgDocument)
    {
        // Tamaño y posición del rectángulo principal en el SVG
        float rectX = 370;
        float rectY = 91;
        float rectWidth = 680;
        float rectHeight = 230;

        // Margen dentro del rectángulo para los elementos
        float marginX = 20;
        float marginY = 20;

        // Tamaño fijo de cada estado
        float stateWidth = 100;
        float stateHeight = 50;

        // Espacio entre los estados
        var stateSpacingX = (rectWidth - 2 * marginX - stateWidth * aggregateStateTransitions.Count) /
                            (aggregateStateTransitions.Count - 1);

        // Calcula la posición inicial para el primer estado
        var currentStateX = rectX + marginX;
        var currentStateY = rectY + marginY;

        // Obtén el grupo stateGroup del documento SVG
        var stateGroup = svgDocument.GetElementById<SvgGroup>("stateGroup");

        // Diccionario para almacenar la posición de cada estado
        var statePositions = new Dictionary<string, (float x, float y)>();


        // Lista de colores para alternar
        var colors = new List<Color>
        {
            ColorTranslator.FromHtml("#f2798b"),
            ColorTranslator.FromHtml("#a8ccf6"),
            ColorTranslator.FromHtml("#d8f79c")
        };

        // Iterador de colores
        var colorIndex = 0;

        // Itera sobre cada StateTransition para dibujar los estados y sus transiciones
        foreach (var stateTransition in aggregateStateTransitions)
        {
            // Guarda la posición del estado
            statePositions[stateTransition.State] = (currentStateX, currentStateY);

            // Crea el rectángulo para representar el estado
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

            // Alterna el color
            colorIndex = (colorIndex + 1) % colors.Count;

            // Añade el nombre del estado
            var stateText = new SvgText
            {
                X = new SvgUnitCollection { new(currentStateX + stateWidth / 2) },
                Y = new SvgUnitCollection
                    { new(currentStateY + stateHeight / 2) }, // Posición ajustable para centrar verticalmente el texto
                FontSize = new SvgUnit(12),
                FontWeight = SvgFontWeight.Bold,
                Fill = new SvgColourServer(Color.Black),
                TextAnchor = SvgTextAnchor.Middle,
                // Clase de estilo definida en SVG
                ID = "stateText_" + Guid.NewGuid().ToString("N")
            };
            stateText.Text = stateTransition.State;
            stateGroup.Children.Add(stateText);

            // Actualiza la posición x para el próximo estado
            currentStateX += stateWidth + stateSpacingX;
        }

        // Itera sobre cada StateTransition para dibujar las flechas de transición
        foreach (var stateTransition in aggregateStateTransitions)
        {
            var (startX, startY) = statePositions[stateTransition.State];

            foreach (var transition in stateTransition.Transitions)
                if (statePositions.TryGetValue(transition.To, out var endPosition))
                {
                    var (endX, endY) = endPosition;

                    // Calcula los puntos de inicio y fin en los bordes de los rectángulos
                    var arrowStartX = startX + stateWidth;
                    var arrowStartY = startY + stateHeight / 2;
                    var arrowEndX = endX;
                    var arrowEndY = endY + stateHeight / 2;

                    // Dibuja la línea de la flecha
                    var arrowLine = new SvgLine
                    {
                        StartX = new SvgUnit(arrowStartX),
                        StartY = new SvgUnit(arrowStartY),
                        EndX = new SvgUnit(arrowEndX),
                        EndY = new SvgUnit(arrowEndY),
                        Stroke = new SvgColourServer(Color.Black),
                        StrokeWidth = new SvgUnit(1),
                        // Añade el marcador definido en el SVG
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
        var formattedElement = System.Text.RegularExpressions.Regex.Replace(element, "([a-z])([A-Z])", "$1 $2");

        // Convert kebab-case to words separated by spaces
        formattedElement = formattedElement.Replace("-", " ");

        // Ensure the first letter is capitalized
        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(formattedElement.ToLower());
    }
}