using BenchmarkDotNet.Attributes;
using DDDCanvasCreator.Creators;
using DDDCanvasCreator.Models.BoundedContextBasic;

namespace DDDCanvasCreator.Benchmarks;

/// <summary>
///     Benchmark class for testing the performance of BcBasicCreator methods.
/// </summary>
/// <remarks>
///     Benchmark Results:
///     | Method               | Mean     | Error    | StdDev   |
///     |--------------------- |---------:|---------:|---------:|
///     | OriginalParseYaml    | 36.15 us | 0.661 us | 0.618 us |
///     | AlternativeParseYaml | 56.68 us | 1.053 us | 1.081 us |
/// </remarks>
public class BcBasicCreatorBenchmark
{
    private readonly BcBasicCreator _creator = new();

    private const string YamlContent = @"
boundedContexts:
  - name: Sales
    models:
      - name: Customer
        type: CoreConcept
      - name: Product
        type: SubConcept
      - name: Address
        type: SubConcept
  - name: Accounts
    models:
      - name: Account
        type: CoreConcept
      - name: User
        type: CoreConcept
      - name: Payment Details
        type: SubConcept
      - name: Transaction History
        type: SubConcept
      - name: Account Settings
        type: SubConcept
  - name: Inventory
    models:
      - name: Stock
        type: CoreConcept
      - name: Warehouse
        type: CoreConcept
      - name: Supplier
        type: CoreConcept
      - name: Item
        type: SubConcept
      - name: Inventory Levels
      - name: Restocking
      - name: Shipping
      - name: Returns
      - name: Damaged Goods
";
    
}