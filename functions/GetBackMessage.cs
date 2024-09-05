using Spectre.Console;

namespace FunctionCode;
class GetBackMessageProcedure
{
    public static void GetBackMessage(int n)
    {
        System.Console.CursorVisible = false;
        for(int i = 0; i < n; i++)
        {
            AnsiConsole.WriteLine();
        }
        AnsiConsole.MarkupLine(" [bold #FFA07A]Pritisnite tipku za povratak...[/]");
        System.Console.ReadKey(true);
        System.Console.CursorVisible = true;
    }
}
