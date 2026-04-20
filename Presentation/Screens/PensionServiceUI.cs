// Console UI for the pension calculator feature.
using Project.Application.Dtos.Pension;
using Project.Application.Services.Pension;
using Project.Presentation.Abstractions;
using Project.Presentation.Rendering;

namespace Project.Presentation.Screens;

/// <summary>Interactive console UI for the pension calculator.</summary>
public sealed class PensionServiceUI : IScreen
{
    private readonly PensionCalculator _calculator;

    /// <summary>Creates a new <see cref="PensionServiceUI"/>.</summary>
    public PensionServiceUI(PensionCalculator calculator) => _calculator = calculator;

    /// <inheritdoc />
    public Task RunAsync(CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader("PENSION CALCULATOR");

        var birthDate = ReadBirthDate();
        var result    = _calculator.Calculate(new PensionCalculationRequest(birthDate));

        Console.WriteLine();

        if (result.IsFailure)
        {
            ResultRenderer.RenderError(result.Error!.Value);
            ConsoleHelper.Pause();
            return Task.CompletedTask;
        }

        var v = result.Value;

        ConsoleHelper.WriteInfo($"Din alder              : {v.Age}");
        ConsoleHelper.WriteInfo($"Folkepensionsalder     : {v.PensionAge}");
        ConsoleHelper.WriteInfo($"Pensionsdato           : {v.PensionDate:dd-MM-yyyy}");
        ConsoleHelper.WriteInfo($"Tid til pension        : {v.TimeUntilPension}");

        if (v.Warning is not null)
        {
            Console.WriteLine();
            ConsoleHelper.WriteWarning(v.Warning);
        }

        Console.WriteLine();
        ConsoleHelper.Pause();
        return Task.CompletedTask;
    }

    private static DateOnly ReadBirthDate()
    {
        while (true)
        {
            Console.Write("  Indtast din fødselsdato (dd-MM-yyyy): ");
            string input = ConsoleHelper.ReadLine();

            if (DateOnly.TryParseExact(input, "dd-MM-yyyy", out var date))
                return date;

            ConsoleHelper.WriteWarning("Ugyldigt datoformat.");
        }
    }
}
