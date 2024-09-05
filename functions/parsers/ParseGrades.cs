using HtmlAgilityPack;
using Newtonsoft.Json;

namespace FunctionCode;
class ParseGradesProcedure
{
    public static async Task ParseGradesData(HttpClient client)
    {
        string jsonData = File.ReadAllText("classData.json");
        List<ClassData>? classDataList = JsonConvert.DeserializeObject<List<ClassData>>(jsonData);

        var IDDataMap = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>>();

        foreach (var classData in classDataList)
        {
            await client.GetAsync($"https://ocjene.skole.hr/class_action/{classData.CourseID}/course");

            FetchData fetchData = new(client);
            string HTMLContent = await fetchData.GetHTML("https://ocjene.skole.hr/grade/all");

            HtmlDocument document = new();
            document.LoadHtml(HTMLContent);

            var predmeti = document.DocumentNode.SelectNodes("//div[@class='flex-table new-grades-table']");
            if (predmeti == null)
            {
                continue;
            }

            var predmetiDataMap = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

            foreach (var predmet in predmeti)
            {
                string predmetTitle = predmet.GetAttributeValue("data-action-id", string.Empty);
                var elementiVrednovanjaMap = new Dictionary<string, Dictionary<string, string>>();

                var redovi = predmet.SelectNodes(".//div[contains(@class, 'row ') and not(contains(@class, 'header'))]");
                if (redovi == null)
                {
                    continue;
                }

                foreach (var red in redovi)
                {
                    string datum = red.SelectSingleNode(".//div[@class='cell']/span")?.InnerText.Trim() ?? "N/A";
                    var box = red.SelectSingleNode(".//div[@class='box']");

                    var elementVrednovanjaNode = box?.SelectSingleNode(".//div[@class='cell'][1]/span");
                    string elementVrednovanja = elementVrednovanjaNode?.InnerText.Trim() ?? "bilješke";

                    var ocjenaNode = box?.SelectSingleNode(".//div[@class='cell'][2]/span");
                    string ocjena = ocjenaNode?.InnerText.Trim() ?? "*";

                    if (!string.IsNullOrEmpty(elementVrednovanja) && !string.IsNullOrEmpty(ocjena))
                    {
                        if (!elementiVrednovanjaMap.TryGetValue(elementVrednovanja, out Dictionary<string, string>? value))
                        {
                            value = ([]);
                            elementiVrednovanjaMap[elementVrednovanja] = value;
                        }

                        value[datum] = ocjena;
                    }
                }

                predmetiDataMap[predmetTitle] = elementiVrednovanjaMap;
            }

            IDDataMap[classData.CourseID] = predmetiDataMap;
        }

        string outputJson = JsonConvert.SerializeObject(IDDataMap, Formatting.Indented);
        File.WriteAllText("gradesData.json", outputJson);
    }
}
