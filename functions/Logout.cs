using Spectre.Console;

namespace FunctionCode;
class LogoutProcedure
{
    public static async Task Logout(HttpClient client, string logoutUrl)
    {
        HttpResponseMessage postResponse = await client.GetAsync(logoutUrl);

        if (postResponse.RequestMessage.RequestUri.AbsoluteUri != "https://ocjene.skole.hr/login")
        {
            return;
        }
        AnsiConsole.MarkupLine("\n [bold #17B169]Uspješna odjavljeni.[/]");
    }
}