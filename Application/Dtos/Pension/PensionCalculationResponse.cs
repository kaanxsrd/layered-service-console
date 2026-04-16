// Output DTO for pension calculation: age, years remaining, and an optional warning.
namespace Project.Application.Dtos.Pension;

/// <summary>Result of a pension calculation.</summary>
/// <param name="Age">The user's current age in whole years.</param>
/// <param name="YearsUntilPension">Years remaining until reaching state pension age.</param>
/// <param name="Warning">Optional advisory message, e.g. when fewer than five years remain.</param>
public sealed record PensionCalculationResponse(
    int     Age,
    int     YearsUntilPension,
    string? Warning
);
