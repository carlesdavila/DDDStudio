// See https://aka.ms/new-console-template for more information

using DDDCanvasCreator.Creators;

Console.WriteLine("Hello, World!");
var outputPath = args.Length > 0 ? args[0] : Path.Combine(Directory.GetCurrentDirectory(), "bounded_contexts.svg");

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

var generator = new BcBasicCreator();
generator.CreateSvg(contexts, outputPath);