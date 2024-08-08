using DDDCanvasCreator.Models;
using DDDCanvasCreator.Services;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DDDCanvasCreator;

public class DDDCanvas
{
    public void GenerateSvg(string yamlFilePath, string outputFilePath, DddConfig config)
    {
        if (string.IsNullOrEmpty(yamlFilePath) || !File.Exists(yamlFilePath))
            throw new FileNotFoundException("The YAML file was not found.", yamlFilePath);
        
        var yamlContent = File.ReadAllText(yamlFilePath);
        var processor = YamlProcessorFactory.CreateProcessor(yamlContent);
        processor.ProcessYamlAndGenerateSvg(yamlContent, outputFilePath, config);
    }
    
    public static DddConfig LoadConfig(string configFilePath)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();

        var config = new DddConfig();

        if (File.Exists(configFilePath))
        {
            using var reader = new StreamReader(configFilePath);
            var loadedConfig = deserializer.Deserialize<DddConfig>(reader);
            config = loadedConfig;
        }
        else
        {
            Console.WriteLine("Configuration file not found. Using default configuration.");
        }

        return config;
    }
}