using Svg;

namespace DDDCanvasCreator.Extensions;

public static class SvgDocumentExtensions
{
    public static void AddRectWithText(this SvgDocument svgDoc, SvgRectangle rect, string text, string textClass)
    {
        svgDoc.Children.Add(rect);

        var posX = (int)rect.X.Value;
        var posY = (int)rect.Y.Value;
        var modelWidth = (int)rect.Width.Value;
        var modelHeight = (int)rect.Height.Value;

        var switchElement = new SvgSwitch();
        var foreignObject = new SvgForeignObject();
        foreignObject.CustomAttributes.Add("x", posX.ToString());
        foreignObject.CustomAttributes.Add("y", posY.ToString());
        foreignObject.CustomAttributes.Add("width", modelWidth.ToString());
        foreignObject.CustomAttributes.Add("height", modelHeight.ToString());

        var div = new NonSvgElement("div", "http://www.w3.org/1999/xhtml")
        {
            Content = text,
            CustomAttributes =
            {
                { "class", textClass },
                { "style", $"width: {modelWidth}px; height: {modelHeight}px; font-size: {modelHeight / 4}px;" }
            }
        };
        foreignObject.Nodes.Add(div);
        switchElement.Children.Add(foreignObject);

        var svgText = new SvgText(text)
        {
            X = [posX + modelWidth / 2],
            Y = [posY + modelHeight / 2],
            TextAnchor = SvgTextAnchor.Middle
        };
        svgText.CustomAttributes.Add("class", textClass);
        switchElement.Children.Add(svgText);

        svgDoc.Children.Add(switchElement);
    }
}