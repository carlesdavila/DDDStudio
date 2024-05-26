using YamlDotNet.Serialization;

namespace DDDCanvasCreator.Models.BoundedContextBasic;

public class BoundedContextsBasic()
{
    // This gets set by all of the code paths that read the application
    [YamlIgnore]
    public FileInfo Source { get; set; } = default!;
    
    public List<BoundedContext> BoundedContexts { get; set; } = [];
}