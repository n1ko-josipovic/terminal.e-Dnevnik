using Spectre.Console;


namespace InterfaceCode;
class BannerProcedure
{
    public static void Banner()
    {
        AnsiConsole.Write(
            new Markup("[bold #FFF]\n" +
                       "      ██╗ ██████╗ ███████╗██╗  ██╗ █████╗ \n" +
                       "      ██║██╔═══██╗██╔════╝██║ ██╔╝██╔══██╗\n" +
                       "      ██║██║   ██║███████╗█████╔╝ ███████║\n" +
                       " ██   ██║██║   ██║╚════██║██╔═██╗ ██╔══██║\n" +
                       " ╚█████╔╝╚██████╔╝███████║██║  ██╗██║  ██║\n" +
                       "  ╚════╝  ╚═════╝ ╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝\n" +
                       "\n" +
                       "[/]")
        );

        AnsiConsole.MarkupLine("[bold] Dobrodošli u [bold #32de84 underline]Terminal e-Dnevnik[/] v.1.0.0![/]");
        AnsiConsole.MarkupLine("");
    }
}