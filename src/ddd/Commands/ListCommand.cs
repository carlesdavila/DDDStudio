using Cocona;

namespace ddd.Commands;

public class ListCommand
{
    [Command("list", Description = "List All DDD Artifacts")]
    public void Command()
    {
        const string rootPath = "DDD";
        const string subdomainSuffix = "Subdomain";
        const string contextSuffix = "Context";
        const string aggregateSuffix = "Aggregate";

        if (!Directory.Exists(rootPath))
        {
            Console.WriteLine($"The directory '{rootPath}' does not exist.");
            return;
        }

        Console.WriteLine("Subdomains");

        foreach (var subdomainDir in Directory.GetDirectories(rootPath))
        {
            var subdomainName = Path.GetFileName(subdomainDir);
            if (subdomainName.EndsWith(subdomainSuffix))
                subdomainName = subdomainName.Substring(0, subdomainName.Length - subdomainSuffix.Length);
            Console.WriteLine($"  - {subdomainName}");

            var contextDirs = Directory.GetDirectories(subdomainDir);
            if (contextDirs.Length > 0)
            {
                Console.WriteLine("    Bounded Contexts");
                foreach (var contextDir in contextDirs)
                {
                    var contextName = Path.GetFileName(contextDir);
                    if (contextName.EndsWith(contextSuffix))
                        contextName = contextName.Substring(0, contextName.Length - contextSuffix.Length);
                    Console.WriteLine($"      - {contextName}");

                    var aggregateDirs = Directory.GetDirectories(contextDir);
                    if (aggregateDirs.Length > 0)
                    {
                        Console.WriteLine("        Aggregates");
                        foreach (var aggregateDir in aggregateDirs)
                        {
                            var aggregateName = Path.GetFileName(aggregateDir);
                            if (aggregateName.EndsWith(aggregateSuffix))
                                aggregateName =
                                    aggregateName.Substring(0, aggregateName.Length - aggregateSuffix.Length);
                            Console.WriteLine($"          - {aggregateName}");
                        }
                    }
                }
            }
        }
    }
}