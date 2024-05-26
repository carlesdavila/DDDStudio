// See https://aka.ms/new-console-template for more information

using DDDCanvasCreator.Models.BoundedContextBasic;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

Console.WriteLine("Hello, World!");

var contexts = new List<BoundedContext>
{
    new()
    {
        Name = "Sales",
        Color = "#0000FF", // Blue
        Models = new List<Model>
        {
            new() { Name = "Customer", Type = "AggregateRoot" },
            new() { Name = "Product", Type = "AggregateRoot" },
            new() { Name = "Address", Type = "ValueObject" }
        }
    },
    new()
    {
        Name = "Inventory",
        Color = "#008000", // Green
        Models = new List<Model>
        {
            new() { Name = "Product", Type = "AggregateRoot" },
            new() { Name = "Warehouse", Type = "AggregateRoot" },
            new() { Name = "InventoryQuantity", Type = "ValueObject" }
        }
    }
};


// Define la ruta del archivo YAML de salida
var yamlFilePath = "bounded_contexts_output.yaml";

// Serializa la lista a YAML
var yamlContent = SerializeToYaml(contexts);


// Escribe el contenido YAML en el archivo
File.WriteAllText(yamlFilePath, yamlContent);


string? SerializeToYaml(List<BoundedContext> boundedContexts)
{
    var serializer = new SerializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    var root = new { bounded_contexts = contexts };

    return serializer.Serialize(root);
}