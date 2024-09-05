using Spectre.Console;

using CommandCode;
using FunctionCode;

namespace InterfaceCode;
class StartProcedure
{
    public static async Task Start(HttpClient client)
    {

        while (true)
        {
            BannerProcedure.Banner();

            var startChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(" [bold cyan]Izaberite opciju:[/]")
                    .PageSize(5)
                    .MoreChoicesText("[grey](Prikaz svih opcija: [[" + "\u2193" + "]])[/]")
                    .AddChoices(
                    [
                        "Login",
                        "Info",
                        "[red]Exit[/]",
                    ]));

            switch (startChoice)
            {
                case "Login":
                    bool loginSuccess = await LoginProcedure.Login(client);

                    if (loginSuccess)
                    {
                        AnsiConsole.Clear();
                        BannerProcedure.Banner();
                        return;
                    }

                    break;

                case "Info":
                    InfoProcedure.Info();
                    break;

                case "[red]Exit[/]":
                    Environment.Exit(0);
                    break;

                default:
                    break;
            }
        }
    }
}

