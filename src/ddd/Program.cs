using System.Reflection;
using Cocona;
using DDDCanvasCreator;

var app = CoconaApp.Create();


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


app.AddCommand("version", () =>
    {
        ShowLogo("starting...!");
        ShowVersion();
    })
    .WithDescription("Client Version");
app.Run();


return;


static void ShowLogo(string message)
{
    var logo = $"\n        {message}";
    logo += @"

  _____  _____  _____     _____ _             _ _        
 |  __ \|  __ \|  __ \   / ____| |           | (_)       
 | |  | | |  | | |  | | | (___ | |_ _   _  __| |_  ___   
 | |  | | |  | | |  | |  \___ \| __| | | |/ _` | |/ _ \  
 | |__| | |__| | |__| |  ____) | |_| |_| | (_| | | (_) | 
 |_____/|_____/|_____/ _|_____/ \__|\__,_|\__,_|_|\___/  
               
";

    Console.WriteLine(logo);
}

static void ShowVersion()
{
    var assembly = Assembly.GetExecutingAssembly();

    var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                  ?? assembly.GetName().Version?.ToString()
                  ?? "Version not found";
    var cleanVersion = version.Split('+')[0];

    Console.WriteLine($"Version: {cleanVersion}");
}