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

        var aggregatePath = Path.Combine(Constants.MainPath, subdomain, context, $"{name}.yaml");

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
  description: ""Some Description.""
  stateTransitions: 
    - state: Inactive
      transitions:
        - to: Active
    - state: Active
      transitions:
        - to: Cancelled
    - state: Cancelled
  enforcedInvariants: 
    - ""Order must have at least one OrderItem.""
    - ""Order total must be recalculated when items are added or removed.""
  correctivePolicies: 
    - ""If an OrderItem is out of stock, notify the customer and adjust the order or issue a refund.""
    - ""If the order status is not updated due to a system failure, retry the update process.""
  handledCommands: 
    - PlaceOrder
    - Confirm
    - Ship
    - Complete
    - Cancel
  createdEvents: 
    - OrderPlaced
    - OrderConfirmed
    - OrderShipped
    - OrderCompleted
    - OrderCancelled
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