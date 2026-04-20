namespace Project.Domain.ValueObjects;

/// <summary>
/// Remaining time until a person reaches their pension date, expressed as whole years
/// plus leftover months. Carries display logic so callers don't repeat the formatting.
/// </summary>
public readonly record struct PensionDuration(int Years, int Months)
{
    /// <summary>Sentinel used when the person has already reached pension age.</summary>
    public static readonly PensionDuration Elapsed = new(0, 0);

    /// <summary>True when there is no time left (pension age already reached).</summary>
    public bool IsElapsed => Years == 0 && Months == 0;

    public override string ToString() => IsElapsed
        ? "Du er allerede pensionsberettiget"
        : Months == 0
            ? $"{Years} år"
            : $"{Years} år og {Months} måned(er)";
}
