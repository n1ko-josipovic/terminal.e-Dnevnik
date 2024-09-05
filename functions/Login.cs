using System.Text.RegularExpressions;

using HtmlAgilityPack;
using Newtonsoft.Json;
using Spectre.Console;

namespace FunctionCode;
class LoginProcedure
{
    public static async Task<bool> Login(HttpClient client)
    {
        AnsiConsole.Write(" ");
        string username = AnsiConsole.Ask<string>(" Upiši [bold cyan]korisnièko ime[/]: ");
        AnsiConsole.Write(" ");
        string password = AnsiConsole.Prompt(
            new TextPrompt<string>(" Upiši [bold cyan]zaporku[/]: ")
                .PromptStyle("red")
                .Secret('\u2219'));

        if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
        {
            username = username.TrimEnd();
            if (!username.EndsWith("@skole.hr", StringComparison.OrdinalIgnoreCase))
            {
                username += "@skole.hr";
            }

            string loginUrl = "https://ocjene.skole.hr/login";
            HttpResponseMessage getResponse = await client.GetAsync(loginUrl);

            if (getResponse.IsSuccessStatusCode)
            {
                string HTMLContent = await getResponse.Content.ReadAsStringAsync();
                string csrfTokenPattern = @"<input[^>]+name=['""]csrf_token['""][^>]+value=['""]([^'""]+)['""]";
                Match match = Regex.Match(HTMLContent, csrfTokenPattern);

                if (match.Success)
                {
                    string csrfToken = match.Groups[1].Value;

                    var formData = new FormUrlEncodedContent(
                    [
                        new KeyValuePair<string, string>("username", username),
                        new KeyValuePair<string, string>("password", password),
                        new KeyValuePair<string, string>("csrf_token", csrfToken)
                    ]);

                    HttpResponseMessage postResponse = await client.PostAsync(loginUrl, formData);

                    #pragma warning disable CS8602
                    if (postResponse.RequestMessage.RequestUri.AbsoluteUri != loginUrl)
                    {
                        AnsiConsole.MarkupLine("\n [bold #17B169]Uspješna prijava.[/]");

                        HTMLContent = await postResponse.Content.ReadAsStringAsync();

                        HtmlDocument html = new();
                        html.LoadHtml(HTMLContent);

                        var nameNode = html.DocumentNode.SelectSingleNode("//div[@class='user-name']/span");
                        string? name = nameNode.InnerText.Trim();

                        var personalData = new
                        {
                            Name = name,
                            Role = "Uèenik",
                            Username = username
                        };

                        string json = JsonConvert.SerializeObject(personalData, Formatting.Indented);

                        string filePath = "personalData.json";
                        File.WriteAllText(filePath, json);

                        return true;
                    }
                    #pragma warning restore CS8602
                }
            }

        }

        AnsiConsole.MarkupLine("\n [bold red]Neuspješna prijava.[/]\n");
        Thread.Sleep(250);

        AnsiConsole.Clear();

        return false;
    }
}
