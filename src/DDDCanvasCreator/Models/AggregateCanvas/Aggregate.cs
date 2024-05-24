namespace DDDCanvasCreator.Models.AggregateCanvas;

public class Aggregate
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<StateTransition> StateTransitions { get; set; }
}