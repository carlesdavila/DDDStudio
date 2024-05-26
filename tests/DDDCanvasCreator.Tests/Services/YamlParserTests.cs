using DDDCanvasCreator.Services;

namespace DDDCanvasCreator.Tests.Services;

public class YamlParserTests
{
    [Fact]
    public void CanParseBoundedContext()
    {

      var input = @"
boundedContexts:
  - name: Sales
    color: '#0000FF'
    models:
      - name: Customer
        type: AggregateRoot
      - name: Product
        type: AggregateRoot
      - name: Address
        type: ValueObject
  - name: Inventory
    color: '#008000'
    models:
      - name: Product
        type: AggregateRoot
      - name: Warehouse
        type: AggregateRoot
      - name: InventoryQuantity
        type: ValueObject";

        
      using var parser = new YamlParser(input);
      var app = parser.ParseBoundedContextsBasic();
    }
}