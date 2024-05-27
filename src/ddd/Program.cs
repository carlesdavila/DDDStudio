using Cocona;
using ddd.Commands;

var app = CoconaApp.Create();

app.AddCommands<AboutCommand>();

app.AddCommands<InitCommand>();

app.AddCommands<AddSubdomainCommand>();

app.AddCommands<AddContextCommand>();

app.AddCommands<AddAggregateCommand>();

app.AddCommands<GenerateDocsCommand>();

app.Run();