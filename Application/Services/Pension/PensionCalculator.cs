// Pension calculator: pure business logic over a DateOnly birth date.
using Project.Application.Abstractions;
using Project.Application.Common;
using Project.Application.Dtos.Pension;
using Project.Application.Mapping;
using Project.Domain.ValueObjects;

namespace Project.Application.Services.Pension;

/// <summary>
/// Calculates current age and time until Danish state pension (folkepension).
/// The applicable pension age is derived from the official schedule — born after a
/// threshold date means a higher pension age (e.g. born ≥ 1 jan 1979 → age 71.5).
/// </summary>
public sealed class PensionCalculator
{
    // Official Danish folkepension schedule (Beskæftigelsesministeriet).
    // Each entry: (earliest birth date that qualifies, pension age).
    // Checked in descending order — first match wins.
    private static readonly (DateOnly From, FolkepensionAge Age)[] Schedule =
    [
        (new DateOnly(1996, 7,  1), FolkepensionAge.From(74.0)),
        (new DateOnly(1992, 1,  1), FolkepensionAge.From(73.5)),
        (new DateOnly(1987, 7,  1), FolkepensionAge.From(73.0)),
        (new DateOnly(1983, 1,  1), FolkepensionAge.From(72.5)),
        (new DateOnly(1979, 1,  1), FolkepensionAge.From(71.5)),
        (new DateOnly(1975, 1,  1), FolkepensionAge.From(71.0)),
        (new DateOnly(1971, 1,  1), FolkepensionAge.From(70.0)),
        (new DateOnly(1967, 1,  1), FolkepensionAge.From(69.0)),
        (new DateOnly(1963, 1,  1), FolkepensionAge.From(68.0)),
        (new DateOnly(1955, 7,  1), FolkepensionAge.From(67.0)),
    ];

    private static readonly FolkepensionAge FallbackPensionAge = FolkepensionAge.From(65.0);
    private const int WarningThresholdYears = 5;

    private readonly IClock _clock;

    /// <summary>Creates a new <see cref="PensionCalculator"/>.</summary>
    public PensionCalculator(IClock clock) => _clock = clock;

    /// <summary>Returns the <see cref="FolkepensionAge"/> that applies to the given birth date.</summary>
    public static FolkepensionAge GetPensionAge(DateOnly birthDate)
    {
        foreach (var (from, age) in Schedule)
        {
            if (birthDate >= from)
                return age;
        }
        return FallbackPensionAge;
    }

    /// <summary>Calculates age and pension data for the given request.</summary>
    public Result<PensionCalculationResponse> Calculate(PensionCalculationRequest request)
    {
        var today = _clock.Today;

        if (request.BirthDate >= today)
            return Result<PensionCalculationResponse>.Failure(
                Errors.Validation("Fødselsdatoen må ikke være i fremtiden."));

        int rawAge = today.Year - request.BirthDate.Year;
        if (request.BirthDate > today.AddYears(-rawAge))
            rawAge--;

        var age        = Age.From(rawAge);
        var pensionAge = GetPensionAge(request.BirthDate);
        var pensionDate = request.BirthDate
            .AddYears(pensionAge.WholeYears)
            .AddMonths(pensionAge.ExtraMonths);

        PensionDuration duration;
        if (pensionDate <= today)
        {
            duration = PensionDuration.Elapsed;
        }
        else
        {
            int years     = pensionDate.Year - today.Year;
            int monthDiff = pensionDate.Month - today.Month;

            if (pensionDate.Day < today.Day)
                monthDiff--;

            if (monthDiff < 0)
            {
                years--;
                monthDiff += 12;
            }

            duration = new PensionDuration(years, monthDiff);
        }

        string? warning = duration.Years < WarningThresholdYears
            ? "Du skal tjekke din pensionsopsparing."
            : null;

        return Result<PensionCalculationResponse>.Success(
            PensionMappings.ToResponse(age, pensionAge, pensionDate, duration, warning));
    }
}
