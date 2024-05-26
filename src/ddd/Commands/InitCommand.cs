using Cocona;

namespace ddd.Commands;

public class InitCommand
{
    [Command("init", Description = "Initializes the DDD project")]
    public void Command()
    {
        string[] directories =
        {
            "DDD/Subdomains",
            "docs"
        };

        foreach (var dir in directories)
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                Console.WriteLine($"Created directory: {dir}");
            }

        Console.WriteLine("Project initialized.");
    }
}