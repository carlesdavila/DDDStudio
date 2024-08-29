using Cocona;

namespace ddd.Commands;

public class InitCommand
{
    [Command("init", Description = "Initializes the DDD project")]
    public void Command()
    {
        CreateDirectories();
        CreateDddYamlFile();
        CreateSubdomainsYamlFile();
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
        var yamlFilePath = "ddd.yaml";
        if (!File.Exists(yamlFilePath))
        {
            File.WriteAllText(yamlFilePath, Constants.DddYamlContent);
            Console.WriteLine($"Created file: {yamlFilePath}");
        }
        else
        {
            Console.WriteLine($"File {yamlFilePath} already exists.");
        }
    }

    private void CreateSubdomainsYamlFile()
    {
        var yamlFilePath = Path.Combine(Constants.MainPath, "Subdomains.yaml");
        if (!File.Exists(yamlFilePath))
        {
            File.WriteAllText(yamlFilePath, Constants.SubdomainsYamlContent);
            Console.WriteLine($"Created file: {yamlFilePath}");
        }
        else
        {
            Console.WriteLine($"File {yamlFilePath} already exists.");
        }
    }
}