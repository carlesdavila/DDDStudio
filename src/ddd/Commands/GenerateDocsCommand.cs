﻿using Cocona;
using DDDCanvasCreator;

namespace ddd.Commands;

public class GenerateDocsCommand
{
    [Command(Description = "Generate SVG documentation from YAML files.")]
    public void GenerateDocs()
    {
        var yamlFiles = Directory.GetFiles(Constants.MainPath, "*.yaml", SearchOption.AllDirectories);

        const string outputFolder = "docs";
        if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);

        foreach (var yamlFile in yamlFiles)
            try
            {
                var dddCanvas = new DDDCanvas();
                var outputFilePath = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(yamlFile) + ".svg");
                dddCanvas.GenerateSvg(yamlFile, outputFilePath);
                Console.WriteLine($"SVG documentation generated for '{Path.GetFileName(yamlFile)}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error generating SVG documentation for '{Path.GetFileName(yamlFile)}':\n" +
                    $"Exception Type: {ex.GetType()}\n" +
                    $"Message: {ex.Message}\n" +
                    $"Stack Trace: {ex.StackTrace}\n" +
                    $"Inner Exception: {ex.InnerException}");
            }
    }
}