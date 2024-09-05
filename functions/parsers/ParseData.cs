namespace FunctionCode;
class ParseProcedure
{
    public static async Task ParseData(HttpClient client)
    { 
        await ParseClassProcedure.ParseClassData(client);

        await ParseGradesProcedure.ParseGradesData(client);

        await ParseNotesProcedure.ParseNotesData(client);
    }
}