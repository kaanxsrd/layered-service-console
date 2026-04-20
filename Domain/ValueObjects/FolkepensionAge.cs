namespace Project.Domain.ValueObjects;

/// <summary>
/// The applicable Danish state pension age (folkepensionsalder) for a given birth cohort.
/// Encapsulates fractional ages like 71.5 (71 years + 6 months) and exposes the
/// breakdown needed to compute a pension date.
/// </summary>
public readonly record struct FolkepensionAge(double Value)
{
    /// <summary>Creates a <see cref="FolkepensionAge"/> from a schedule value (e.g. 71.5).</summary>
    public static FolkepensionAge From(double value) => new(value);

    /// <summary>Whole years component (e.g. 71 for 71.5).</summary>
    public int WholeYears => (int)Value;

    /// <summary>Remaining months after <see cref="WholeYears"/> (e.g. 6 for 71.5).</summary>
    public int ExtraMonths => (int)Math.Round((Value - WholeYears) * 12);

    /// <summary>Display string without the unit: "67", "71,5".</summary>
    public string Display => Value % 1 == 0 ? $"{(int)Value}" : $"{Value:0.#}";

    public override string ToString() => $"{Display} år";
}
