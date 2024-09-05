using Newtonsoft.Json;
using Spectre.Console;

using FunctionCode;

namespace InterfaceCode;
class SelectClassProcedure
{
	private static async Task<List<ClassData>> LoadClassesFromFile(string filePath)
	{
		var classDataList = new List<ClassData>();

		var jsonData = await File.ReadAllTextAsync(filePath);
        classDataList = JsonConvert.DeserializeObject<List<ClassData>>(jsonData);

		return classDataList;
	}

	public static async Task<string> SelectClass()
	{
		string filePath = "classData.json";

		var classes = await LoadClassesFromFile(filePath);

		var classOptions = new List<string>();
		var classDictionary = new Dictionary<string, string>();

		foreach (var item in classes)
		{
			var displayText = $"{item.Class}.{item.Department} {item.School}";
			classOptions.Add(displayText);
			classDictionary[displayText] = item.CourseID;
		}

		var selectedClass = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title(" [bold cyan]Izaberite razred:[/]")
				.PageSize(3)
				.MoreChoicesText("[grey](Prikaz svih razreda: [[" + "\u2193" + "]])[/]")
				.AddChoices(classOptions)
		);

		// CourseID...
		return classDictionary[selectedClass];
	}
}
