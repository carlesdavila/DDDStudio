using DDDCanvasCreator.Creators;

namespace DDDCanvasCreator.Models.BoundedContextBasic;

public class BoundedContext
{
    public string? Name { get; set; }
    public List<Model> Models { get; set; } = [];
}