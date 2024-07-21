using Cocona;

namespace ddd.Commands;

public class InitCommand
{
    [Command("init", Description = "Initializes the DDD project")]
    public void Command()
    {
        CreateDirectories();
        CreateDddYamlFile();
        Console.WriteLine("Project initialized.");
    }

    private void CreateDirectories()
    {
        string[] directories =
        {
            Constants.MainPath,
            "docs"
        };

        foreach (var dir in directories)
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                Console.WriteLine($"Created directory: {dir}");
            }
    }

    private void CreateDddYamlFile()
    {
        // Crear el fichero ddd.yaml en la raíz del proyecto
        var yamlFilePath = "ddd.yaml";
        if (!File.Exists(yamlFilePath))
        {
            var yamlContent = @"
# DDD Project Configuration

# Base class or interface that identifies an aggregate
AggregateBase: ""MyNamespace.IAggregateRoot""

# List of colors for different states of aggregates
AggregateStatesColors:
  - ""#FF0000""
  - ""#00FF00""
  - ""#0000FF""

# Color for handled commands of aggregates
HandledCommandsColor: ""#FFFF00""

# Color for created events
CreatedEventsColor: ""#FF00FF""
";
            File.WriteAllText(yamlFilePath, yamlContent);
            Console.WriteLine($"Created file: {yamlFilePath}");
        }
        else
        {
            Console.WriteLine($"File {yamlFilePath} already exists.");
        }
    }
}