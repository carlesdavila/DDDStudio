using Cocona;

namespace ddd.Commands;

public class AddSubdomainCommand
{
    [Command(Description = "Add a new subdomain and generate a base YAML file.")]
    public void Subdomain([Argument] string name)
    {
        const string suffix = "Subdomain";
        if (!name.EndsWith(suffix)) name += suffix;
        
        var subdomainPath = Path.Combine(Constants.MainPath, name);
        if (!Directory.Exists(subdomainPath))
        {
            Directory.CreateDirectory(subdomainPath);
            Console.WriteLine($"Subdomain '{name}' added.");

            var yamlContent = @"boundedContexts:
  - name: Sales
    models:
      - name: Customer
        type: CoreConcept
      - name: Product
        type: SubConcept
      - name: Address
        type: SubConcept
  - name: Inventory
    models:
      - name: Product
        type: CoreConcept
      - name: Warehouse
        type: SubConcept
      - name: Inventory Quantity
        type: SubConcept
";
            var yamlFilePath = Path.Combine(Constants.MainPath, name, $"{name}.yaml");
            File.WriteAllText(yamlFilePath, yamlContent);
            Console.WriteLine($"Created base YAML file for subdomain '{name}' at '{yamlFilePath}'.");
        }
        else
        {
            Console.WriteLine($"Subdomain '{name}' already exists.");
        }
    }
}