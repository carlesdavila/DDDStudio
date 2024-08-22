namespace DDDCanvasCreator.Models.Subdomains;

public class Subdomain
{
    public required string Name { get; set; }
    public required string Type { get; set; }
    public string? Purpose { get; set; }
    public string? Context { get; set; }
}