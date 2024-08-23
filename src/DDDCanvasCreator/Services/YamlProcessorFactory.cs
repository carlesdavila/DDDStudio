﻿using DDDCanvasCreator.Creators;

namespace DDDCanvasCreator.Services;

public static class YamlProcessorFactory
{
    public static IYamlProcessor CreateProcessor(string yamlContent)
    {
        // Logic to determine which processor to use based on the YAML content
        return yamlContent switch
        {
            _ when yamlContent.Contains("boundedContexts") => new BcBasicCreator(),
            _ when yamlContent.Contains("aggregate") => new AggregateCanvasCreator(),
            _ when yamlContent.Contains("subdomains") => new SubdomainsCanvasCreator(),
            _ => throw new NotSupportedException("Unsupported YAML format")
        };
    }
}