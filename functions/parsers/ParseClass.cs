using HtmlAgilityPack;
using Newtonsoft.Json;

namespace FunctionCode;
class ParseClassProcedure
{
    public static async Task ParseClassData(HttpClient client)
    {
        FetchData fetchData = new(client);
        string HTMLContent = await fetchData.GetHTML("https://ocjene.skole.hr/class");
        var classDataList = ParseClassHTML(HTMLContent);

        string jsonData = JsonConvert.SerializeObject(classDataList, Formatting.Indented);

        File.WriteAllText("classData.json", jsonData);
    }

    public static List<ClassData> ParseClassHTML(string HTMLContent)
    {
        HtmlDocument document = new();
        document.LoadHtml(HTMLContent);

        List<ClassData> classDataList = [];

        var classes = document.DocumentNode.SelectNodes("//div[contains(@class, 'class-menu-vertical')]");

        foreach (var classNode in classes)
        {
            var classInfoNode = classNode.SelectSingleNode(".//div[contains(@class, 'class-info')]");

            var schoolYear = classInfoNode.SelectSingleNode(".//span[contains(@class, 'class-schoolyear')]").InnerText.Trim();

            var className = classInfoNode.SelectSingleNode(".//span[contains(@class, 'bold')]").InnerText.Trim();
            var classParts = className.Split('.');
            var classNumber = classParts[0];
            var department = classParts[1].ToUpper();

            var schoolName = classInfoNode.SelectSingleNode(".//span[contains(@class, 'school-name')]").InnerText.Trim();

            var courseId = classInfoNode.GetAttributeValue("data-action-id", string.Empty);

            var overallGradeNode = classNode.SelectSingleNode(".//li[contains(@class, 'overall-grade')]//span[contains(@class, 'bold')]");
            string overallGrade = overallGradeNode != null ? overallGradeNode.InnerText.Trim() : "0.00";

            var shortSchoolName = GetShortSchoolName(schoolName);

            classDataList.Add(new ClassData
            {
                Class = classNumber,
                Department = department,
                School = shortSchoolName,
                Year = schoolYear,
                CourseID = courseId,
                OverallGrade = overallGrade
            });
        }

        return classDataList;
    }


    public static string GetShortSchoolName(string schoolName)
    {
        var words = schoolName.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
        string shortName = string.Join("", words.Select(w => w[0])).ToUpper();
        return shortName;
    }

}
public class ClassData
{
    public string Class { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string School { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public string CourseID { get; set; } = string.Empty;
    public string OverallGrade { get; set; } = string.Empty;
}
