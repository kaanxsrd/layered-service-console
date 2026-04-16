using Project.Services;

namespace Project.UI;

/// <summary>
/// Console interface for the pension calculator service.
/// </summary>
public static class PensionServiceUI
{
    private static readonly PensionCalculator _calculator = new();

    /// <summary>
    /// Reads the user's birth date, runs the calculation and prints the result.
    /// </summary>
    public static void Run()
    {
        ConsoleHelper.WriteHeader("PENSION CALCULATOR");

        DateOnly birthDate = ReadBirthDate();
        PensionResult result = _calculator.Calculate(birthDate);

        Console.WriteLine();
        ConsoleHelper.WriteInfo($"Din alder              : {result.Age} år");
        ConsoleHelper.WriteInfo($"År til pension (alder {PensionCalculator.PensionAge}): {result.YearsUntilPension} år");

        if (result.Warning is not null)
        {
            Console.WriteLine();
            ConsoleHelper.WriteWarning(result.Warning);
        }

        Console.WriteLine();
        ConsoleHelper.Pause();
    }

    private static DateOnly ReadBirthDate()
    {
        while (true)
        {
            Console.Write("  Indtast din fødselsdato (dd-MM-yyyy): ");
            string input = Console.ReadLine() ?? string.Empty;

            if (DateOnly.TryParseExact(input, "dd-MM-yyyy", out DateOnly date))
                return date;

            ConsoleHelper.WriteWarning("Ugyldigt datoformat.");
        }
    }
}
