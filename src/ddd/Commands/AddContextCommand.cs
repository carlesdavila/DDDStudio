using Cocona;

namespace ddd.Commands;

public class AddContextCommand
{
    [Command(Description = "Add a new bounded context to a subdomain.")]
    public void Context([Argument] string name, [Option('s')] string subdomain)
    {
        const string suffix = "Context";
        const string subdomainSuffix = "Subdomain";
        if (!name.EndsWith(suffix)) name += suffix;
        if (!subdomain.EndsWith(subdomainSuffix)) subdomain += subdomainSuffix;


        var contextPath = Path.Combine(Constants.MainPath, subdomain, name);

        if (!Directory.Exists(contextPath))
        {
            Directory.CreateDirectory(contextPath);
            Console.WriteLine($"Bounded Context '{name}' added to subdomain '{subdomain}'.");
        }
        else
        {
            Console.WriteLine($"Bounded Context '{name}' already exists in subdomain '{subdomain}'.");
        }
    }
}