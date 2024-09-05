using System.Text.Json;
using Spectre.Console;
using InterfaceCode;

namespace CommandCode;
class NotesProcedure
{
    public static async Task Notes(string ID)
    {

        var jsonData = await File.ReadAllTextAsync("notesData.json");
        var notesDataList = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(jsonData);

        AnsiConsole.Clear();
        BannerProcedure.Banner();

        notesDataList.TryGetValue(ID, out var notes);

        if (notes != null && notes.Count != 0)
        {
            var panels = new List<Panel>();

            foreach (var note in notes)
            {
                string content = note.Value != null && note.Value.Count > 0
                    ? string.Join("\n", note.Value.Values)
                    : "[grey]Nema dostupnih podataka.[/]";

                var panel = new Panel("\n" + content + "\n")
                    .Header($"[bold #50C878]{note.Key}[/]")
                    .Border(BoxBorder.Rounded);

                panels.Add(panel);
            }

            AnsiConsole.Live(new Panel("[bold blue]Bilješke[/]"))
                .Start(ctx =>
                {
                    var currentPanels = new List<Panel>();

                    foreach (var panel in panels)
                    {
                        currentPanels.Add(panel);

                        var outerPanel = new Panel(new Rows(currentPanels.ToArray()))
                            .Header("[bold cyan]Bilješke[/]")
                            .Border(BoxBorder.Rounded)
                            .Padding(1, 1, 4, 0);

                        ctx.UpdateTarget(outerPanel);
                        ctx.Refresh();
                        Thread.Sleep(150);
                    }
                });
        }

        FunctionCode.GetBackMessageProcedure.GetBackMessage(0);

        AnsiConsole.Clear();
    }
}
