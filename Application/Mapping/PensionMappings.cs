// Explicit mappings for the pension calculation feature.
using Project.Application.Dtos.Pension;
using Project.Domain.ValueObjects;

namespace Project.Application.Mapping;

/// <summary>Explicit, hand-written mappings for pension DTOs.</summary>
public static class PensionMappings
{
    /// <summary>Builds a <see cref="PensionCalculationResponse"/> from strongly-typed values.</summary>
    public static PensionCalculationResponse ToResponse(
        Age age, FolkepensionAge pensionAge, DateOnly pensionDate,
        PensionDuration timeUntilPension, string? warning) =>
        new(age, pensionAge, pensionDate, timeUntilPension, warning);
}
