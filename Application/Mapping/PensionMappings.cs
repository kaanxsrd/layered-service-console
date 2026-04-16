// Explicit mappings for the pension calculation feature.
using Project.Application.Dtos.Pension;

namespace Project.Application.Mapping;

/// <summary>Explicit, hand-written mappings for pension DTOs.</summary>
public static class PensionMappings
{
    /// <summary>Builds a <see cref="PensionCalculationResponse"/> from raw values.</summary>
    /// <param name="age">Calculated age in whole years.</param>
    /// <param name="yearsUntilPension">Years remaining until state pension age.</param>
    /// <param name="warning">Optional advisory message.</param>
    public static PensionCalculationResponse ToResponse(int age, int yearsUntilPension, string? warning) =>
        new(age, yearsUntilPension, warning);
}
