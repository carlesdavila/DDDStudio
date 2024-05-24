namespace DDDCanvasCreator.Models.AggregateCanvas;

public class StateTransition
{
    public string Initial { get; set; }
    public List<string> Transitions { get; set; }
}