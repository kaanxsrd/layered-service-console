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
        Result<PensionResult> result = _calculator.Calculate(birthDate);

        Console.WriteLine();

        if (!result.IsSuccess)
        {
            ConsoleHelper.WriteError(result.Error);
            ConsoleHelper.Pause();
            return;
        }

        ConsoleHelper.WriteInfo($"Din alder              : {result.Value.Age} år");
        ConsoleHelper.WriteInfo($"År til pension (alder {PensionCalculator.PensionAge}): {result.Value.YearsUntilPension} år");

        if (result.Value.Warning is not null)
        {
            Console.WriteLine();
            ConsoleHelper.WriteWarning(result.Value.Warning);
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