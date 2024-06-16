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
            "DDD",
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