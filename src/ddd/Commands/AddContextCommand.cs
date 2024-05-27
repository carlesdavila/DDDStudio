using Cocona;

namespace ddd.Commands;

public class AddContextCommand
{
    [Command(Description = "Add a new bounded context to a subdomain.")]
    public void AddContext(string name, string subdomain)
    {
        var contextPath = Path.Combine("DDD", "Subdomains", subdomain, "BoundedContexts", name);
        string[] contextDirectories =
        {
            "Aggregates"
        };

        if (!Directory.Exists(contextPath))
        {
            Directory.CreateDirectory(contextPath);
            foreach (var subDir in contextDirectories) Directory.CreateDirectory(Path.Combine(contextPath, subDir));
            Console.WriteLine($"Bounded Context '{name}' added to subdomain '{subdomain}'.");
        }
        else
        {
            Console.WriteLine($"Bounded Context '{name}' already exists in subdomain '{subdomain}'.");
        }
    }
}