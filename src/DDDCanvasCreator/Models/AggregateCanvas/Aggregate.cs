using YamlDotNet.Serialization;

namespace DDDCanvasCreator.Models.AggregateCanvas;

public class Aggregate
{
    [YamlIgnore]
    public FileInfo Source { get; set; } = default!;
    public string Name { get; set; }
    public string Description { get; set; }
    public List<StateTransition> StateTransitions { get; set; }
}