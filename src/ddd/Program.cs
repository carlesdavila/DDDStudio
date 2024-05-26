using Cocona;
using ddd.Commands;
using DDDCanvasCreator;

var app = CoconaApp.Create();

app.AddCommands<AboutCommand>();

app.AddCommands<InitCommand>();

app.AddCommand("GenerateSvgs", ([Argument] string inputDirectory, string outputDirectory) =>
    {
        var dddCanvas = new DDDCanvas();

        if (!Directory.Exists(inputDirectory))
        {
            Console.WriteLine($"The input directory '{inputDirectory}' does not exist.");
            return;
        }

        if (!Directory.Exists(outputDirectory)) Directory.CreateDirectory(outputDirectory);

        var yamlFiles = Directory.GetFiles(inputDirectory, "*.yaml");
        foreach (var yamlFile in yamlFiles)
        {
            var fileName = Path.GetFileNameWithoutExtension(yamlFile);
            var outputFilePath = Path.Combine(outputDirectory, $"{fileName}.svg");

            try
            {
                dddCanvas.GenerateSvg(yamlFile, outputFilePath);
                Console.WriteLine($"Generated SVG for {yamlFile} -> {outputFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to generate SVG for {yamlFile}: {ex.Message}");
            }
        }
    })
    .WithDescription("Generate all svgs from yaml files");

app.Run();