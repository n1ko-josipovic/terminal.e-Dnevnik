namespace CommandCode;
class DeleteProcedure
{
    public static void Delete()
    {
        File.Delete("classData.json");
        File.Delete("gradesData.json");
        File.Delete("notesData.json");
        File.Delete("personalData.json");
    }
}