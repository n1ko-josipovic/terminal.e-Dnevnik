using System.Text;

using FunctionCode;
using InterfaceCode;

namespace Program;
class Program
{
    public static async Task Main()
    {
        System.Console.OutputEncoding = Encoding.UTF8;
        HttpClient client = new();

        while (true)
        {
            await StartProcedure.Start(client);

            await ParseProcedure.ParseData(client);

            string selectedClass = await SelectClassProcedure.SelectClass();

            await MenuProcedure.Menu(client, selectedClass);
        }
    }
}
