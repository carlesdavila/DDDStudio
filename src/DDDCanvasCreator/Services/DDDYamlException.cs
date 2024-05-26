using YamlDotNet.Core;

namespace DDDCanvasCreator.Services;

public class DDDYamlException : Exception
{
    public DDDYamlException(string message)
        : base(message)
    {
    }

    public DDDYamlException(Mark start, string message)
        : this(start, message, null)
    {
    }

    public DDDYamlException(Mark start, string message, Exception? innerException)
        : base($"Error parsing YAML: ({start.Line}, {start.Column}): {message}", innerException)
    {
    }

    public DDDYamlException(Mark start, string message, Exception? innerException, FileInfo fileInfo)
        : base($"Error parsing '{fileInfo.Name}': ({start.Line}, {start.Column}): {message}", innerException)
    {
    }

    public DDDYamlException(string message, Exception? inner)
        : base(message, inner)
    {
    }
}