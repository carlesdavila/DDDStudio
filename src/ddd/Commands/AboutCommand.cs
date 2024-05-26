using System.Reflection;
using Cocona;

namespace ddd.Commands;

public class AboutCommand
{
    [Command("about")]
    public void Command()
    {
        ShowLogo("starting...!");
        ShowVersion();
    }

    private static void ShowLogo(string message)
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

    private static void ShowVersion()
    {
        var assembly = Assembly.GetExecutingAssembly();

        var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                      ?? assembly.GetName().Version?.ToString()
                      ?? "Version not found";
        var cleanVersion = version.Split('+')[0];

        Console.WriteLine($"Version: {cleanVersion}");
    }
}