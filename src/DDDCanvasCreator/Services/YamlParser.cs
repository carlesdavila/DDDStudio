using DDDCanvasCreator.Models.AggregateCanvas;
using DDDCanvasCreator.Models.BoundedContextBasic;
using DDDCanvasCreator.Models.Subdomains;
using DDDCanvasCreator.Parsers.AggregateParser;
using DDDCanvasCreator.Parsers.BoundedContextsParser;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DDDCanvasCreator.Services;

public class YamlParser : IDisposable
{
    private readonly FileInfo? _fileInfo;
    private readonly TextReader _reader;
    private readonly YamlStream _yamlStream;

    public YamlParser(string yamlContent, FileInfo? fileInfo = null)
        : this(new StringReader(yamlContent), fileInfo)
    {
    }

    public YamlParser(FileInfo fileInfo)
        : this(fileInfo.OpenText(), fileInfo)
    {
    }

    internal YamlParser(TextReader reader, FileInfo? fileInfo = null)
    {
        _reader = reader;
        _yamlStream = new YamlStream();
        _fileInfo = fileInfo;
    }

    public void Dispose()
    {
        _reader.Dispose();
    }

    public Aggregate ParseAggregate()
    {
        try
        {
            _yamlStream.Load(_reader);
        }
        catch (YamlException ex)
        {
            if (_fileInfo != null)
                throw new DDDYamlException(ex.Start, $"Unable to parse '{_fileInfo.Name}'. See inner exception.", ex,
                    _fileInfo);

            throw new DDDYamlException(ex.Start, "Unable to parse YAML.  See inner exception.", ex);
        }

        var aggregate = new Aggregate();

        var document = _yamlStream.Documents[0];
        var node = document.RootNode;
        ThrowIfNotYamlMapping(node, _fileInfo);

        aggregate.Source = _fileInfo!;

        AggregateParser.HandleAggregate((YamlMappingNode)node, aggregate);


        return aggregate;
    }

    public BoundedContextsBasic ParseBoundedContextsBasic()
    {
        try
        {
            _yamlStream.Load(_reader);
        }
        catch (YamlException ex)
        {
            if (_fileInfo != null)
                throw new DDDYamlException(ex.Start, $"Unable to parse '{_fileInfo.Name}'. See inner exception.", ex,
                    _fileInfo);

            throw new DDDYamlException(ex.Start, "Unable to parse YAML.  See inner exception.", ex);
        }

        var app = new BoundedContextsBasic();

        // TODO assuming first document.
        var document = _yamlStream.Documents[0];
        var node = document.RootNode;
        ThrowIfNotYamlMapping(node, _fileInfo);

        app.Source = _fileInfo!;

        BoundedContextsBasicParser.HandleBoundedContextsBasic((YamlMappingNode)node, app);

        return app;
    }

    public static void ThrowIfNotYamlMapping(YamlNode node, FileInfo? fileInfo = null)
    {
        if (node.NodeType != YamlNodeType.Mapping)
        {
            if (fileInfo != null)
                throw new DDDYamlException(node.Start,
                    $"UnexpectedType!  expected: {YamlNodeType.Mapping.ToString()}, actual: {node.NodeType.ToString()}"
                    , null, fileInfo);
            throw new DDDYamlException(node.Start,
                $"UnexpectedType!  expected: {YamlNodeType.Mapping.ToString()}, actual: {node.NodeType.ToString()}");
        }
    }

    public static void ThrowIfNotYamlSequence(string key, YamlNode node)
    {
        if (node.NodeType != YamlNodeType.Sequence)
            throw new DDDYamlException(node.Start, $"ExpectedYamlSequence:{key}");
    }


    public static List<object> GetSequence(YamlNode node)
    {
        if (node.NodeType != YamlNodeType.Sequence)
            throw new DDDYamlException(node.Start,
                $"UnexpectedType! expected: {YamlNodeType.Sequence.ToString()}, actual: {node.NodeType.ToString()}");

        var sequence = new List<object>();

        foreach (var item in (YamlSequenceNode)node)
            sequence.Add(item.NodeType switch
            {
                YamlNodeType.Scalar => GetScalarValue(item),
                YamlNodeType.Mapping => GetDictionary(item),
                YamlNodeType.Sequence => GetSequence(item),
                _ => throw new DDDYamlException(item.Start,
                    $"UnexpectedType! expected: {YamlNodeType.Sequence.ToString()}, actual: {item.NodeType.ToString()}")
            });

        return sequence;
    }

    public static Dictionary<string, object> GetDictionary(YamlNode node)
    {
        if (node.NodeType != YamlNodeType.Mapping)
            throw new DDDYamlException(node.Start,
                $"UnexpectedType!  expected: {YamlNodeType.Mapping.ToString()}, actual: {node.NodeType.ToString()}");

        var dictionary = new Dictionary<string, object>();

        foreach (var mapping in (YamlMappingNode)node)
        {
            var key = GetScalarValue(mapping.Key);

            dictionary[key] = mapping.Value.NodeType switch
            {
                YamlNodeType.Scalar => GetScalarValue(key, mapping.Value)!,
                YamlNodeType.Mapping => GetDictionary(mapping.Value),
                YamlNodeType.Sequence => GetSequence(mapping.Value),

                _ => throw new DDDYamlException(mapping.Value.Start,
                    $"UnexpectedType!  expected: {YamlNodeType.Mapping.ToString()}, actual: {mapping.Value.NodeType.ToString()}")
            };
        }

        return dictionary;
    }

    public static string GetScalarValue(YamlNode node)
    {
        if (node.NodeType != YamlNodeType.Scalar)
            throw new DDDYamlException(node.Start,
                $"UnexpectedType!  expected: {YamlNodeType.Mapping.ToString()}, actual: {node.NodeType.ToString()}"
            );

        return ((YamlScalarNode)node).Value!;
    }

    public static string GetScalarValue(string key, YamlNode node)
    {
        if (node.NodeType != YamlNodeType.Scalar)
            throw new DDDYamlException(node.Start, $"ExpectedYamlScalar Key: {key}");

        return ((YamlScalarNode)node).Value!;
    }

    public SubdomainCollection ParseSubdomainCollection()
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();

        var subdomains = deserializer.Deserialize<SubdomainCollection>(_reader);

        return subdomains;
    }
}