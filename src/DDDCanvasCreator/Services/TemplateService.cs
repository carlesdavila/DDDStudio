using System.Reflection;
using Svg;

namespace DDDCanvasCreator.Services;

public static class TemplateService
{
    private static string ReadEmbeddedResource(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourceName);

        if (stream == null) throw new FileNotFoundException($"Resource '{resourceName}' not found.");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public static SvgDocument GetAggregateSvgDocument()
    {
        const string resourceName = "DDDCanvasCreator.Templates.aggregate-template.svg";
        var svgContent = ReadEmbeddedResource(resourceName);
        return SvgDocument.FromSvg<SvgDocument>(svgContent);
    }

    public static SvgDocument GetContextSvgDocument()
    {
        const string resourceName = "DDDCanvasCreator.Templates.contexts-template.svg";
        var svgContent = ReadEmbeddedResource(resourceName);
        return SvgDocument.FromSvg<SvgDocument>(svgContent);
    }
    
    public static SvgDocument GetSubdomainSvgDocument()
    {
        const string resourceName = "DDDCanvasCreator.Templates.subdomains-template.svg";
        var svgContent = ReadEmbeddedResource(resourceName);
        return SvgDocument.FromSvg<SvgDocument>(svgContent);
    }
}