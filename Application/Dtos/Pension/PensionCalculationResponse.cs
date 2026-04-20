// Output DTO for pension calculation: age, pension schedule data, and an optional warning.
using Project.Domain.ValueObjects;

namespace Project.Application.Dtos.Pension;

/// <summary>Result of a pension calculation.</summary>
/// <param name="Age">The user's current age.</param>
/// <param name="PensionAge">Applicable folkepension age (e.g. 67, 71.5).</param>
/// <param name="PensionDate">The exact date the user reaches state pension age.</param>
/// <param name="TimeUntilPension">Remaining time expressed as years + months.</param>
/// <param name="Warning">Optional advisory message, e.g. when fewer than five years remain.</param>
public sealed record PensionCalculationResponse(
    Age             Age,
    FolkepensionAge PensionAge,
    DateOnly        PensionDate,
    PensionDuration TimeUntilPension,
    string?         Warning
);
