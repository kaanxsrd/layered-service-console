using System.Text.RegularExpressions;
using SchoolProject.Data;
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

    internal static void PrintMenu()
    {
        Console.WriteLine("\n  Vælg en mulighed:");
        Console.WriteLine("  1) Vis alle medarbejdere");
        Console.WriteLine("  2) Vis kvindelige medarbejdere");
        Console.WriteLine("  3) Vis mandlige medarbejdere");
        Console.WriteLine("  4) Vis medarbejdere med 5 eller færre år til pension");
        Console.WriteLine("  5) Registrere ny medarbejder");
        Console.WriteLine("  6) Vis løndata oversigt");
        Console.WriteLine("  7) Afslut");
        Console.Write("\n  Dit valg: ");
    }

    internal static void Pause()
    {
        Console.WriteLine("\n  Tryk på en tast for at vende tilbage til menuen...");
        Console.ReadKey(intercept: true);
    }

    internal static void PrintDtoList(List<EmployeeDTO> list)
    {
        Console.WriteLine($"\n  {"Navn",-28} {"Køn",-5} {"Alder",5}  {"Afdeling",-22}");
        Console.WriteLine($"  {new string('─', 64)}");
        foreach (var e in list)
            Console.WriteLine($"  {$"{e.ForNavn} {e.EfterNavn}",-28} {e.Køn,-5} {e.Age,5}  {FormatEnumName(e.Afdeling.ToString()),-22}");
        Console.WriteLine($"  {new string('─', 64)}");
        Console.WriteLine($"  Total: {list.Count} medarbejdere");
    }

    internal static void PrintRecordList(List<EmployeeRecord> list)
    {
        if (list.Count == 0)
        {
            Console.WriteLine("  (ingen)");
            return;
        }
        Console.WriteLine($"\n  {"Navn",-28} {"Køn",-5} {"Alder",5}  {"Afdeling",-22}");
        Console.WriteLine($"  {new string('─', 64)}");
        foreach (var r in list)
            Console.WriteLine($"  {$"{r.ForNavn} {r.EfterNavn}",-28} {r.Køn,-5} {r.Alder,5}  {FormatEnumName(r.Afdeling.ToString()),-22}");
        Console.WriteLine($"  {new string('─', 64)}");
        Console.WriteLine($"  Total: {list.Count} medarbejdere");
    }

    internal static void PrintNearRetirementList(List<EmployeeRecord> list)
    {
        if (list.Count == 0)
        {
            Console.WriteLine("  (ingen)");
            return;
        }
        Console.WriteLine($"\n  {"Navn",-28} {"Køn",-5} {"Alder",5}  {"Afdeling",-22}  {"Fratrædelsesbonus",18}");
        Console.WriteLine($"  {new string('─', 85)}");
        foreach (var r in list)
        {
            decimal bonus = SalaryData.GetPensionBonus(r.Afdeling, r.Køn);
            Console.WriteLine($"  {$"{r.ForNavn} {r.EfterNavn}",-28} {r.Køn,-5} {r.Alder,5}  {FormatEnumName(r.Afdeling.ToString()),-22}  {bonus,16:N0} kr");
        }
        Console.WriteLine($"  {new string('─', 85)}");
        Console.WriteLine($"  Total: {list.Count} medarbejdere");
    }

    internal static void PrintSalaryData()
    {
        var deptValues = Enum.GetValues<Department>();
        Console.WriteLine($"\n  {"Afdeling",-24} {"Gns. løn kvinde",16} {"Gns. løn mand",16} {"Bonus mand",16} {"Bonus kvinde",16}");
        Console.WriteLine($"  {new string('─', 92)}");
        for (int i = 0; i < deptValues.Length; i++)
        {
            string dept = FormatEnumName(deptValues[i].ToString());
            decimal avgF   = SalaryData.Løndata[i, 0];
            decimal avgM   = SalaryData.Løndata[i, 1];
            decimal bonusM = SalaryData.Løndata[i, 2];
            decimal bonusF = SalaryData.Løndata[i, 3];
            Console.WriteLine($"  {dept,-24} {avgF,14:N0} kr {avgM,14:N0} kr {bonusM,14:N0} kr {bonusF,14:N0} kr");
        }
        Console.WriteLine($"  {new string('─', 92)}");
    }

    internal static string FormatEnumName(string name) =>
        Regex.Replace(name, "(?<!^)([A-Z])", " $1");

    internal static EmployeeDTO? ReadEmployeeFromInput()
    {
        Console.Write("\n  Fornavn               : ");
        var forNavn = Console.ReadLine()?.Trim() ?? "";

        Console.Write("  Efternavn             : ");
        var efterNavn = Console.ReadLine()?.Trim() ?? "";

        Console.Write("  Fødselsdato (dd/MM/yyyy): ");
        var dateInput = Console.ReadLine()?.Trim() ?? "";

        var genderValues = Enum.GetValues<Gender>();
        Console.WriteLine("  Køn:");
        for (int i = 0; i < genderValues.Length; i++)
            Console.WriteLine($"    {i + 1}) {genderValues[i]}");
        Console.Write($"  Vælg (1-{genderValues.Length})            : ");
        var genderInput = Console.ReadLine()?.Trim() ?? "";

        var deptValues = Enum.GetValues<Department>();
        Console.WriteLine("  Afdeling:");
        for (int i = 0; i < deptValues.Length; i++)
            Console.WriteLine($"    {i + 1}) {FormatEnumName(deptValues[i].ToString())}");
        Console.Write($"  Vælg (1-{deptValues.Length})            : ");
        var deptInput = Console.ReadLine()?.Trim() ?? "";

        if (!DateOnly.TryParseExact(dateInput, "dd/MM/yyyy", null,
                System.Globalization.DateTimeStyles.None, out var date))
        {
            Error("Ugyldig datoformat – brug dd/MM/yyyy.");
            return null;
        }

        if (!int.TryParse(genderInput, out var genderIdx) || genderIdx < 1 || genderIdx > genderValues.Length)
        {
            Error($"Ugyldigt køn – vælg 1-{genderValues.Length}.");
            return null;
        }
        var gender = genderValues[genderIdx - 1];

        if (!int.TryParse(deptInput, out var deptIdx) || deptIdx < 1 || deptIdx > deptValues.Length)
        {
            Error($"Ugyldig afdeling – vælg 1-{deptValues.Length}.");
            return null;
        }
        var department = deptValues[deptIdx - 1];

        try
        {
            return new EmployeeDTO(forNavn, efterNavn, new BirthDate(date), gender, department);
        }
        catch (ArgumentException ex)
        {
            Error(ex.Message);
            return null;
        }
    }
}
