namespace DDDCanvasCreator.Models;

public class DddConfig
{
    public string AggregateBase { get; set; } = "MyNamespace.IAggregateRoot";
    public List<string> AggregateStatesColors { get; set; } = ["#f2798b", "#a8ccf6", "#d8f79c"];
    public string HandledCommandsColor { get; set; } = "#40C7EA";
    public string CreatedEventsColor { get; set; } = "#FFAA5E";
    public List<string> BoundedContextColors { get; set; } = ["#2c9bf0", "#f2798b", "#ffaa5e"];

    public int BoundedContextWidth { get; set; } = 300;
}