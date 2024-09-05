using Spectre.Console;

namespace CommandCode;
class DownloadProcedure
{
    public static async Task DownloadFile(HttpClient client, string fileUrl, string fileName)
    {
        if (!fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
        {
            fileName += ".pdf";
        }

        string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

        string filePath = Path.Combine(downloadsPath, fileName);
        int count = 1;
        while (File.Exists(filePath))
        {
            string tempFileName = $"{Path.GetFileNameWithoutExtension(fileName)}({count}){Path.GetExtension(fileName)}";
            filePath = Path.Combine(downloadsPath, tempFileName);
            count++;
        }

        byte[] fileData = await client.GetByteArrayAsync(fileUrl);

        await File.WriteAllBytesAsync(filePath, fileData);

        AnsiConsole.MarkupLine(" [green]Datoteka je uspješno preuzeta![/]");
        AnsiConsole.MarkupLine($" [bold]Putanja:[/] [underline blue]{filePath}[/]\n");
    }
}
