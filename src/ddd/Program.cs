using Cocona;
using ddd.Commands;

var app = CoconaApp.Create();

app.AddCommands<InitCommand>();

app.AddSubCommand("add", x =>
    {
        x.AddCommands<AddSubdomainCommand>();
        x.AddCommands<AddContextCommand>();
        x.AddCommands<AddAggregateCommand>();
    })
    .WithDescription("Adds a DDD item");

app.AddCommands<GenerateDocsCommand>();

app.AddCommands<AboutCommand>();

app.AddCommands<ListCommand>();


app.Run();