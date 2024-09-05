using System.Text.Json;

using Spectre.Console;

using InterfaceCode;

namespace CommandCode;
class GradesProcedure
{
    public static async Task Grades(string ID)
    {
        string filePath = "gradesData.json";
        var jsonData = await File.ReadAllTextAsync(filePath);
        var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>>>(jsonData);

        AnsiConsole.Clear();
        BannerProcedure.Banner();

        AnsiConsole.MarkupLine($" [bold underline]Ocjene[/]\n");
        AnsiConsole.MarkupLine(" [italic grey]* oznaèava bilješku[/]\n");
        AnsiConsole.WriteLine();

        data.TryGetValue(ID, out var predmeti);

        if (predmeti != null && predmeti.Count != 0)
        {
            var mjeseci = new List<string> { "9.", "10.", "11.", "12.", "1.", "2.", "3.", "4.", "5.", "6." };
            var mjeseciRimski = new List<string> { "IX.", "X.", "XI.", "XII.", "I.", "II.", "III.", "IV.", "V.", "VI." };

            foreach (var predmet in predmeti)
            {
                AnsiConsole.MarkupLine($" [bold cyan underline]{predmet.Key}[/]\n");

                var table = new Table();
                table.Border(TableBorder.Rounded);

                table.AddColumn("Element vrednovanja");
                foreach (var mjesec in mjeseciRimski)
                {
                    table.AddColumn($"[bold]{mjesec}[/]");
                }

                var normalniRedovi = new List<string[]>();
                var narRedovi = new List<string[]>();

                foreach (var elementVrednovanja in predmet.Value)
                {
                    var row = new List<string> { $"[bold #50C878]{elementVrednovanja.Key}[/]" };

                    var ocjenePoMjesecima = new Dictionary<string, List<string>>();
                    foreach (var datum in elementVrednovanja.Value)
                    {
                        if (datum.Key == "N/A")
                        {
                            continue;
                        }
                        var mjesec = datum.Key.Split('.')[1] + ".";

                        if (!ocjenePoMjesecima.ContainsKey(mjesec))
                        {
                            ocjenePoMjesecima[mjesec] = new List<string>();
                        }
                        ocjenePoMjesecima[mjesec].Add(datum.Value);
                    }

                    foreach (var mjesec in mjeseci)
                    {
                        if (ocjenePoMjesecima.TryGetValue(mjesec, out var ocjene))
                        {
                            row.Add(string.Join(", ", ocjene));
                        }
                        else
                        {
                            row.Add("");
                        }
                    }

                    if (elementVrednovanja.Key == "bilješke")
                    {
                        narRedovi.Add(row.ToArray());
                    }
                    else
                    {
                        normalniRedovi.Add(row.ToArray());
                    }
                }

                foreach (var red in normalniRedovi)
                {
                    table.AddRow(red);
                }

                foreach (var red in narRedovi)
                {
                    table.AddRow(red);
                }

                AnsiConsole.Write(table);
                Console.WriteLine("\n");
            }
        }

        FunctionCode.GetBackMessageProcedure.GetBackMessage(0);

        AnsiConsole.Clear();
    }
}
