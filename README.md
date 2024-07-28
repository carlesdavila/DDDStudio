# ![DDD Studio](https://raw.githubusercontent.com/carlesdavila/DDDStudio/main/logo.png) DDD Studio

![NuGet Version](https://img.shields.io/nuget/v/DDD.Studio)

## Project Description

**DDD Studio** is a tool designed to document artifacts identified in the [Domain-Driven Design Starter Modelling Process](https://github.com/ddd-crew/ddd-starter-modelling-process).
It helps users document key elements such as Bounded Contexts and Aggregates. The tool automatically creates a
structured set of folders and YAML documents, making it easy to document and organize your DDD artifacts. YAML is used
for its simplicity and readability, allowing both humans and machines to easily understand and edit the documents. This
structured approach facilitates continuous collaboration and integration into repositories, supporting ongoing
development efforts.

## Folder Structure Generated

The tool generates a folder and file structure to organize subdomains and bounded contexts in a DDD (Domain-Driven Design) project. Below is the detailed structure:

```plaintext
DDD/
├── BusinessSubdomain/
│   ├── BusinessSubdomain.yaml
│   ├── SalesContext/
│   │   ├── OrderAggregate.yaml
│   │   └── ...
│   └── ...
├── AnotherSubdomain/
│   ├── AnotherSubdomain.yaml
│   ├── ExampleContext/
│   │   ├── SampleAggregate.yaml
│   │   └── ...
│   └── ...
└── ddd.yaml
```
## Documenting Subdomains
If you are using the DDD Starter Modelling Process, you are likely employing collaborative techniques such as Domain Storytelling or Event Storming to identify Bounded Contexts. Once these Bounded Contexts are identified, you can use DDD Studio to document them and their models in YAML format.

### Example of Subdomain
```yaml
boundedContexts:
  - name: Sales
    models:
      - name: Customer
        type: AggregateRoot
      - name: Product
        type: AggregateRoot
      - name: Address
        type: ValueObject
  - name: Inventory
    models:
      - name: Product
        type: AggregateRoot
      - name: Warehouse
        type: AggregateRoot
      - name: InventoryQuantity
        type: ValueObject
```
### Generates the following SVG
![Contexts Sample](https://raw.githubusercontent.com/carlesdavila/DDDStudio/main/images/ContextsSample.svg)

## Example of an Aggregate YAML File

Here is an example of the `OrderAggregate.yaml` file:

```yaml
aggregate:
  name: Order
  description: "An aggregate representing a customer order. This boundary was chosen to encapsulate the complete lifecycle of an order from creation to fulfillment or cancellation. Trade-offs include managing concurrency for high-volume orders."
  stateTransitions:
    - state: Pending
      transitions:
        - to: Shipped
    - state: Shipped
      transitions:
        - to: Completed
    - state: Completed
  enforcedInvariants:
    - "Order must have at least one OrderItem."
    - "Order total must be recalculated when items are added or removed."
  correctivePolicies:
    - "If an OrderItem is out of stock, notify the customer and adjust the order or issue a refund."
    - "If the order status is not updated due to a system failure, retry the update process."
  handledCommands:
    - PlaceOrder
    - Ship
    - Complete
  createdEvents:
    - OrderPlaced
    - OrderShipped
    - OrderCompleted
 ```
## Aggregate Design Canvas
The above YAML example would generate the following [Aggregate Design Canvas](https://github.com/ddd-crew/aggregate-design-canvas) in SVG format:
![Aggregate Design Canvas](https://raw.githubusercontent.com/carlesdavila/DDDStudio/main/images/OrderAggregateSample.svg)

## Installation

## Getting started

The DDD CLI tool helps you manage and document Domain-Driven Design (DDD) projects efficiently. This guide will walk you through the basic usage of the DDD CLI commands.

## Usage

```sh
ddd [command]
```

### Commands

- `init` : Initializes the DDD project.
- `generate-docs` : Generate SVG documentation from YAML files.
- `about` : Provides information about the DDD CLI tool.
- `list` : List all DDD artifacts.
- `add` : Adds a DDD item.

## Initializing a DDD Project

To initialize a new DDD project, use the following command:

```sh
ddd init
```

This will set up the necessary structure for your DDD project.

## Generating Documentation

You can generate SVG documentation from your YAML files using:

```sh
ddd generate-docs
```

Ensure that your YAML files are properly configured before running this command.

## Listing All DDD Artifacts

To list all the DDD artifacts in your project, use:

```sh
ddd list
```

This will display a comprehensive list of all the artifacts you have defined.

## Adding DDD Items

The `add` command allows you to add various DDD items to your project. The usage pattern is:

```sh
ddd add [command]
```

### Add Commands

- `subdomain` : Add a new subdomain and generate a base YAML file.
- `context` : Add a new bounded context to a subdomain.
- `aggregate` : Add a new aggregate to a bounded context of a subdomain.

#### Adding a Subdomain

To add a new subdomain and generate a base YAML file, use:

```sh
ddd add subdomain
```

This command will prompt you to enter the necessary details for the subdomain.

#### Adding a Bounded Context

To add a new bounded context to an existing subdomain, use:

```sh
ddd add context
```

You will be prompted to specify the subdomain to which the context will be added.

#### Adding an Aggregate

To add a new aggregate to a bounded context of a subdomain, use:

```sh
ddd add aggregate
```

This command requires specifying both the subdomain and the bounded context for the new aggregate.

## Getting Help

For more information about the DDD CLI tool and its commands, use the `about` command:

```sh
ddd about
```

This will provide detailed information about the tool and its capabilities.

With these commands, you can efficiently manage and document your DDD projects.

## FAQ
### Why doesn't the 8. Throughput work?
Ah, the infamous 8. Throughput! Well, it turns out it's still stuck in a data traffic jam. We've asked it to hurry up, but it seems to be enjoying its vacation too much. It'll be back soon, we promise!

### Why doesn't the 9. Size work?
The 9. Size is on a strict diet of bits and bytes and hasn't reached its final form yet. We're working hard to help it grow strong and healthy. Thanks for your patience while we let it finish its training!

## License
This project is licensed under the MIT License.

