using DDDCanvasCreator.Models;
using DDDCanvasCreator.Models.Subdomains;
using DDDCanvasCreator.Services;

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

    private void GenerateSubdomainsSvg(List<Subdomain> subdomainsSubdomains, string outputFilePath, DddConfig config)
    {
        throw new NotImplementedException();
    }
}