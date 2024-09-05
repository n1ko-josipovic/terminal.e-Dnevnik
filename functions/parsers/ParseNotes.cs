using System.Text.RegularExpressions;

using HtmlAgilityPack;
using Newtonsoft.Json;

namespace FunctionCode;
class ParseNotesProcedure
{
    public static async Task ParseNotesData(HttpClient client)
    {
        string jsonData = File.ReadAllText("classData.json");
        List<ClassData> classDataList = JsonConvert.DeserializeObject<List<ClassData>>(jsonData);

        Dictionary<string, Dictionary<string, Dictionary<string, string>>> IDDataMap = new();

        foreach (var classData in classDataList)
        {
            await client.GetAsync($"https://ocjene.skole.hr/class_action/{classData.CourseID}/course");

            FetchData fetchData = new(client);
            string HTMLContent = await fetchData.GetHTML("https://ocjene.skole.hr/notes");

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(HTMLContent);

            var titles = document.DocumentNode.SelectNodes("//div[@class='title']");

            Dictionary<string, Dictionary<string, string>> titleDataMap = new();

            foreach (var titleNode in titles)
            {
                string title = CleanData(titleNode.InnerText);

                var sectionTextNode = titleNode.SelectSingleNode("following-sibling::div[@class='section-text']");
                if (sectionTextNode != null)
                {
                    var dataSections = HandleNotesData(sectionTextNode.InnerText);

                    titleDataMap[title] = dataSections;
                }
            }

            IDDataMap[classData.CourseID] = titleDataMap;
        }

        string outputJson = JsonConvert.SerializeObject(IDDataMap, Formatting.Indented);
        File.WriteAllText("notesData.json", outputJson);
    }

    private static string CleanData(string input)
    {
        return Regex.Replace(input, @"\s+", " ").Trim();
    }

    private static Dictionary<string, string> HandleNotesData(string input)
    {
        var dataSections = new Dictionary<string, string>();

        var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.None)
                         .Select(line => CleanData(line));

        string sectionText = string.Join(Environment.NewLine, lines);

        lines = sectionText.Split(new[] { '\r', '\n' }, StringSplitOptions.None)
                               .Select(line => CleanData(line))
                               .Where(line => !string.IsNullOrWhiteSpace(line))
                               .ToList();

        int dataIndex = 1;
        foreach (var line in lines)
        {
            dataSections[$"Data{dataIndex}"] = line;
            dataIndex++;
        }

        return dataSections;
    }
}
