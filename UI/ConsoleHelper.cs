using SchoolProject.Enums;
using SchoolProject.Models;

namespace SchoolProject.UI;

internal static class ConsoleHelper
{
    internal static void Header(string title)
    {
        string border = new('═', title.Length + 4);
        Console.WriteLine($"\n╔{border}╗");
        Console.WriteLine($"║  {title}  ║");
        Console.WriteLine($"╚{border}╝");
    }

    internal static void Section(string title)
    {
        Console.WriteLine($"\n── {title} {new string('─', Math.Max(0, 52 - title.Length))}");
    }

    internal static void Error(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n  ✗ {msg}");
        Console.ResetColor();
    }

    internal static void Success(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n  ✓ {msg}");
        Console.ResetColor();
    }

    internal static void PrintDtoList(List<EmployeeDTO> list)
    {
        Console.WriteLine($"\n  {"Navn",-28} {"Køn",-5} {"Alder",5}");
        Console.WriteLine($"  {new string('─', 40)}");
        foreach (var e in list)
            Console.WriteLine($"  {$"{e.ForNavn} {e.EfterNavn}",-28} {e.Køn,-5} {e.Age,5}");
        Console.WriteLine($"  {new string('─', 40)}");
        Console.WriteLine($"  Total: {list.Count} medarbejdere");
    }

    internal static void PrintRecordList(List<EmployeeRecord> list)
    {
        if (list.Count == 0)
        {
            Console.WriteLine("  (ingen)");
            return;
        }
        Console.WriteLine($"  {"Navn",-28} {"Køn",-5} {"Alder",5}");
        Console.WriteLine($"  {new string('─', 40)}");
        foreach (var r in list)
            Console.WriteLine($"  {$"{r.ForNavn} {r.EfterNavn}",-28} {r.Køn,-5} {r.Alder,5}");
    }

    internal static EmployeeDTO? ReadEmployeeFromInput()
    {
        Console.Write("\n  Fornavn               : ");
        var forNavn = Console.ReadLine()?.Trim() ?? "";

        Console.Write("  Efternavn             : ");
        var efterNavn = Console.ReadLine()?.Trim() ?? "";

        Console.Write("  Fødselsdato (dd/MM/yyyy): ");
        var dateInput = Console.ReadLine()?.Trim() ?? "";

        Console.Write("  Køn (M/F)             : ");
        var kønInput = Console.ReadLine()?.Trim().ToUpperInvariant() ?? "";

        if (!DateOnly.TryParseExact(dateInput, "dd/MM/yyyy", null,
                System.Globalization.DateTimeStyles.None, out var date))
        {
            Error("Ugyldig datoformat – brug dd/MM/yyyy.");
            return null;
        }

        if (!Enum.TryParse<Gender>(kønInput, out var gender))
        {
            Error("Ugyldigt køn – brug M eller F.");
            return null;
        }

        try
        {
            return new EmployeeDTO(forNavn, efterNavn, new BirthDate(date), gender);
        }
        catch (ArgumentException ex)
        {
            Error(ex.Message);
            return null;
        }
    }
}
