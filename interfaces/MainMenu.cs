using System.Text.Json;
using System.Reflection;

using Spectre.Console;
using Newtonsoft.Json.Linq;

using CommandCode;
using FunctionCode;

namespace InterfaceCode;
class MenuProcedure
{
    public static async Task Menu(HttpClient client, string ID)
    {
        await DisplayName();

        while (true)
        {
            await client.GetAsync($"https://ocjene.skole.hr/class_action/{ID}/course");

            string class_ = await GetByCourseID(ID, "Class");
            string department = await GetByCourseID(ID, "Department");
            string school = await GetByCourseID(ID, "School");
            string overallGrade = await GetByCourseID(ID, "OverallGrade");

            var menu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($" [bold cyan]{class_}.{department} {school} - {overallGrade}[/]")
                    .PageSize(5)
                    .MoreChoicesText("[grey](Prikaz svih opcija: [[" + "\u2193" + "]])[/]")
                    .AddChoices([
                    "Razred", "Ocjene", "Bilješke",
                    "Ispiti", "Raspored", "[red]Odjava[/]"
                    ])
            );

            switch (menu)
            {
                case "Razred":
                    ID = await SelectClassProcedure.SelectClass();
                    break;
                case "Ocjene":
                    await GradesProcedure.Grades(ID);

                    BannerProcedure.Banner();
                    await DisplayName();
                    break;
                case "Bilješke":
                    await NotesProcedure.Notes(ID);

                    BannerProcedure.Banner();
                    await DisplayName();
                    break;
                case "Ispiti":
                    string ispitiFileUrl = "https://ocjene.skole.hr/exam/pdf";
                    string ispitiFileName = $"Ispiti_{class_}-{school}";
                    await DownloadProcedure.DownloadFile(client, ispitiFileUrl, ispitiFileName);
                    break;
                case "Raspored":
                    string rasporedFileUrl = "https://ocjene.skole.hr/schedule/pdf";
                    string rasporedFileName = $"Raspored_{class_}-{school}";
                    await DownloadProcedure.DownloadFile(client, rasporedFileUrl, rasporedFileName);
                    break;
                case "[red]Odjava[/]":
                    await LogoutProcedure.Logout(client, "https://ocjene.skole.hr/logout");

                    DeleteProcedure.Delete();
                    AnsiConsole.Clear();
                    return;

                default:
                    break;
            }
        }
    }

    public static async Task<string> GetByCourseID(string courseID, string getString)
    {

        var jsonData = await File.ReadAllTextAsync("classData.json");
        var classes = JsonSerializer.Deserialize<List<ClassData>>(jsonData);

        var matchingClass = classes?.FirstOrDefault(c => c.CourseID == courseID);

        var property = typeof(ClassData).GetProperty(getString, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        var value = property.GetValue(matchingClass)?.ToString();

        return value;
    }

    private static async Task DisplayName()
    {
        var jsonData = await File.ReadAllTextAsync("personalData.json");

        JObject personalData;
        personalData = JObject.Parse(jsonData);

        var personalName = personalData["Name"]?.ToString() ?? "Unknown";
        var role = personalData["Role"]?.ToString() ?? "Unknown";
        var personalUsername = personalData["Username"]?.ToString() ?? "Unknown";

        var lines = new[]
        {
            $"[bold]Ime:[/] {personalName}\n",
            $"[bold]Uloga:[/] {role}\n",
            $"[bold]Korisničko ime:[/] {personalUsername}"
        };

        string displayText = "\n";

        await AnsiConsole.Live(new Panel(""))
            .StartAsync(async ctx =>
            {
                foreach (var line in lines)
                {
                    displayText += line + "\n";

                    ctx.UpdateTarget(new Panel(new Markup(displayText))
                    {
                        Header = new PanelHeader("[bold #FF9951]Osobni podaci[/]"),
                        Border = BoxBorder.Rounded,
                        Expand = false
                    });
                    ctx.Refresh();
                    await Task.Delay(150);
                }
            });

        AnsiConsole.WriteLine();
    }
}
