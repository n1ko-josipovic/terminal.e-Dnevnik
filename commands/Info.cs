using Spectre.Console;

namespace CommandCode;
class InfoProcedure
{
    public static void Info()
    {
        string[] lines =
        [
            "[bold #50C878]O aplikaciji:[/]\n",
            " [bold]Naziv aplikacije:[/] Terminal e-Dnevnik",
            " [bold]Verzija:[/] 1.0.0",
            " [bold]Opis:[/] Pregled e-Dnevnika iz terminala. Sigurno i idealno za upotrebu na javnim mjestima.\n",
            "[bold #50C878]O autoru:[/]\n",
            " [bold]Ime:[/] Niko Josipoviæ",
            " [bold]Kontakt:[/] niko.josipovic3@skole.hr",
            " [bold]Opis:[/] Uèenik Tehnièke škole Ruðera Boškoviæa."
        ];

        string displayText = "\n";

        AnsiConsole.Live(new Panel(""))
            .Start(ctx =>
            {
                foreach (var line in lines)
                {
                    displayText += line + "\n";

                    var panel = new Panel(new Markup(displayText))
                    {
                        Header = new PanelHeader("[bold cyan]Informacije[/]"),
                        Border = BoxBorder.Rounded,
                        Expand = false
                    };

                    ctx.UpdateTarget(panel);
                    ctx.Refresh();
                    Thread.Sleep(100);
                }
            });

        FunctionCode.GetBackMessageProcedure.GetBackMessage(1);

        AnsiConsole.Clear();
    }
}
