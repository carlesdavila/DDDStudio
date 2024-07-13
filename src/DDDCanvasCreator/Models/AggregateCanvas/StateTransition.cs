namespace DDDCanvasCreator.Models.AggregateCanvas;

public class StateTransition
{
    public string State { get; set; }
    public List<Transition> Transitions { get; set; } = [];
}

