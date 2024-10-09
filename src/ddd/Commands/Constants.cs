namespace ddd.Commands;

public static class Constants
{
    public static string MainPath => "DDD";

    public static string DddYamlContent => @"
# DDD Project Configuration

# Create files options
CreateAllSubdomainsFile: true
CreateContextsFile: true
CreateBoundedContextCanvas: true
CreateAggregateCanvas: true

# Base class or interface that identifies an aggregate
AggregateBase: ""MyNamespace.IAggregateRoot""

# List of colors for different states of aggregates
AggregateStatesColors:
  - ""#f2798b""
  - ""#a8ccf6""
  - ""#d8f79c""

# Color for handled commands of aggregates
HandledCommandsColor: ""#40C7EA""

# Color for created events
CreatedEventsColor: ""#FFAA5E""

# Color for created events
BoundedContextWidth: 300

# Color for BoundedContexts
BoundedContextColors:
  - ""#2c9bf0""
  - ""#f2798b""
  - ""#ffaa5e""
";

    public static string SubdomainsYamlContent => @"
subdomains:
  - name: ""Order Management""
    type: ""Core Domain""
    purpose: >
      The Order Management subdomain manages the entire lifecycle of an order, from creation to delivery.
      It ensures a seamless and efficient customer experience, which is critical for maintaining customer satisfaction
      and ensuring the business meets its delivery commitments.
    context: >
      This subdomain operates in close interaction with the Inventory, Logistics, and Billing systems.
      It receives order requests from Sales, checks product availability with Inventory, coordinates delivery with Logistics,
      and processes payments through the Billing system. As a Core Domain, Order Management is crucial to the day-to-day operations,
      directly impacting revenue and customer experience.

  - name: ""Inventory Management""
    type: ""Supporting Domain""
    purpose: >
      The Inventory Management subdomain tracks and manages the availability of products in stock.
      It supports order fulfillment by ensuring accurate inventory levels are maintained, preventing stockouts,
      and facilitating efficient restocking processes.
    context: >
      This subdomain is closely linked to the Order Management and Procurement systems.
      It updates stock levels based on incoming orders and communicates restocking needs to Procurement.
      As a Supporting Domain, it provides essential data for order processing and procurement but does not directly contribute to a competitive advantage.

  - name: ""Customer Support""
    type: ""Generic Domain""
    purpose: >
      The Customer Support subdomain handles customer inquiries, complaints, and support requests.
      It enhances customer satisfaction by providing timely and effective support, contributing to improved customer retention and a positive brand perception.
    context: >
      This subdomain operates independently but interacts with the Order Management and CRM systems.
      It accesses order details from Order Management to assist with customer inquiries and logs customer interactions in the CRM system.
      Although a Generic Domain, it provides essential support services that bolster overall customer satisfaction.
";
}