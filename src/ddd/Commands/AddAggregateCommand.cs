using Cocona;

namespace ddd.Commands;

public class AddAggregateCommand
{
    [Command(Description = "Add a new aggregate to a bounded context of a subdomain.")]
    public void Aggregate([Argument] string name, [Option('s')] string subdomain, [Option('c')] string context)
    {
        const string suffix = "Aggregate";
        if (!name.EndsWith(suffix)) name += suffix;
        
        const string contextSuffix = "Context";
        if (!context.EndsWith(contextSuffix)) context += contextSuffix;
        
        const string subdomainSuffix = "Subdomain";
        if (!subdomain.EndsWith(subdomainSuffix)) subdomain += subdomainSuffix;

        var aggregatePath = Path.Combine(Constants.MainPath, subdomain, context, name, $"{name}.yaml");

        if (!Directory.Exists(Path.Combine(Constants.MainPath, subdomain, context)))
        {
            Console.WriteLine($"Bounded Context '{context}' in subdomain '{subdomain}' does not exist.");
            return;
        }

        if (!File.Exists(aggregatePath))
        {
            var yamlContent = $@"
aggregate:
  name: {name}
  description: 
  state_transitions: 
    - 
  enforced_invariants: 
    - 
  corrective_policies: 
    - 
  handled_commands: 
    - 
  created_events: 
    - 
  throughput: 
    command_handling_rate: 
      average: 
      maximum: 
    total_number_of_clients: 
      average: 
      maximum: 
  size:
    event_growth_rate: 
      average: 
      maximum: 
    lifetime: 
      average: 
      maximum: 
";

            File.WriteAllText(aggregatePath, yamlContent);
            Console.WriteLine(
                $"Aggregate design canvas for '{name}' added to Bounded Context '{context}' in subdomain '{subdomain}'.");
        }
        else
        {
            Console.WriteLine(
                $"Aggregate design canvas for '{name}' already exists in Bounded Context '{context}' of subdomain '{subdomain}'.");
        }
    }
}