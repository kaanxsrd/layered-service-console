// Pension calculator: pure business logic over a DateOnly birth date.
using Project.Application.Abstractions;
using Project.Application.Common;
using Project.Application.Dtos.Pension;
using Project.Application.Mapping;

namespace Project.Application.Services.Pension;

/// <summary>
/// Calculates age and proximity to state pension age (folkepensionsalder).
/// </summary>
public sealed class PensionCalculator
{
    /// <summary>Danish state pension age.</summary>
    public const int PensionAge = 67;

    private const int WarningThresholdYears = 5;

    private readonly IClock _clock;

    /// <summary>Creates a new <see cref="PensionCalculator"/>.</summary>
    public PensionCalculator(IClock clock) => _clock = clock;

    /// <summary>Calculates age and years-until-pension for the given request.</summary>
    /// <param name="request">The calculation request.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> wrapping the response on success, or a validation failure
    /// when the birth date is not in the past.
    /// </returns>
    public Result<PensionCalculationResponse> Calculate(PensionCalculationRequest request)
    {
        var today = _clock.Today;

        if (request.BirthDate >= today)
            return Result<PensionCalculationResponse>.Failure(
                Errors.Validation("Fødselsdatoen må ikke være i fremtiden."));

        int age = today.Year - request.BirthDate.Year;
        if (request.BirthDate > today.AddYears(-age))
            age--;

        int yearsUntilPension = Math.Max(0, PensionAge - age);

        string? warning = yearsUntilPension < WarningThresholdYears
            ? "Du skal tjekke din pension opsparing."
            : null;

        return Result<PensionCalculationResponse>.Success(
            PensionMappings.ToResponse(age, yearsUntilPension, warning));
    }
}
