// See https://aka.ms/new-console-template for more information
using System.CommandLine;
using System.Text.RegularExpressions;
using Dasetaba.Data;

var createOption = new Option<bool>("--create", "Ensures that the database for the context exists.");
var snakeColumnOption = new Option<bool>("--snake-column");
var sourceOption = new Option<string>("--source");
var destOption = new Option<string>("--dest");

var rootCommand = new RootCommand("Sample app for System.CommandLine")
{
    createOption,
    snakeColumnOption,
    sourceOption,
    destOption,
};
rootCommand.SetHandler((create, snake, source, dest) =>
{
    if (create)
    {
        var context = new AppDbContext();
        context.Database.EnsureCreated();
    }
    if (snake)
    {
        var text = File.ReadAllText(source);
        var lines = text.Split(Environment.NewLine);
        var newLines = lines.Where(x => !string.IsNullOrEmpty(x)).Select(x =>
        {
            x = x.Trim();
            var propertyName = Regex.Replace(x, @"^public \w* ", string.Empty).Replace(" { get; set; }", string.Empty).Replace(" = string.Empty;", string.Empty);
            var snakeCase = Regex.Replace(propertyName, "[A-Z]", "_$0").TrimStart('_').ToLower();
            var columnAttribute = @$"[Column(""{snakeCase}"")]";
            return $"{columnAttribute}{Environment.NewLine}{x}{Environment.NewLine}";
        });
        var newText = string.Join(Environment.NewLine, newLines);
        File.WriteAllText(dest, newText);
    }
}, createOption, snakeColumnOption, sourceOption, destOption);

await rootCommand.InvokeAsync(args);

Console.WriteLine("Hello, World!");
