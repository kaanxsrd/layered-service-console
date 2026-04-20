namespace Project.Domain.ValueObjects;

/// <summary>
/// A person's age in whole years. Prevents confusing a bare <c>int</c> age with
/// years-until-pension or any other integer in the same context.
/// </summary>
public readonly record struct Age(int Value)
{
    /// <summary>Creates an <see cref="Age"/> from a pre-validated integer.</summary>
    public static Age From(int value) => new(value);

    /// <summary>True when the person has already reached or passed <paramref name="threshold"/>.</summary>
    public bool HasReached(int threshold) => Value >= threshold;

    public override string ToString() => $"{Value} år";
}
