namespace DDDCanvasCreator.Models;

public class DddConfig
{
    // New properties for deserialization
    public bool CreateAllSubdomainsFile { get; set; } = true;
    public bool CreateContextsFile { get; set; } = true;
    public bool CreateBoundedContextCanvas { get; set; } = true;
    public bool CreateAggregateCanvas { get; set; } = true;
    public string AggregateBase { get; set; } = "MyNamespace.IAggregateRoot";
    public List<string> AggregateStatesColors { get; set; } = ["#f2798b", "#a8ccf6", "#d8f79c"];
    public string HandledCommandsColor { get; set; } = "#40C7EA";
    public string CreatedEventsColor { get; set; } = "#FFAA5E";
    public List<string> BoundedContextColors { get; set; } = ["#2c9bf0", "#f2798b", "#ffaa5e"];
    public int BoundedContextWidth { get; set; } = 300;
    
    public List<string> SubdomainColors { get; set; } = ["#8A2BE2", "#FF1493", "#1E90FF", $"#FF8C00", "#FFD700", "#32CD32"];
    
    public int SubdomainWidth { get; set; } = 300;
    
    public string OutputDirectory { get; set; } = "./docs";

}