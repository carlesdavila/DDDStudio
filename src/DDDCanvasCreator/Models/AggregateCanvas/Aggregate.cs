using YamlDotNet.Serialization;

namespace DDDCanvasCreator.Models.AggregateCanvas;

public class Aggregate
{
    [YamlIgnore]
    public FileInfo Source { get; set; } = default!;
    public string Name { get; set; }
    public string Description { get; set; }
    public List<StateTransition> StateTransitions { get; set; } = [];
    public List<string> EnforcedInvariants { get; set; } = [];
    public List<string> CorrectivePolicies { get; set; } = [];
    public List<string> HandledCommands { get; set; } = [];
    public List<string> CreatedEvents { get; set; } = [];



}