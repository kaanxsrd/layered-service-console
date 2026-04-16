namespace Project.Services;

public class PensionCalculator
{
    /// <summary>Danish state pension age (folkepensionsalder).</summary>
    public const int PensionAge = 67;

    /// <summary>
    /// Calculates age and pension proximity for the given birth date.
    /// </summary>
    /// <param name="birthDate">The user's date of birth.</param>
    /// <returns>
    /// A <see cref="PensionResult"/> with age, years until pension, and an optional
    /// savings warning when fewer than 5 years remain.
    /// </returns>
    public Result<PensionResult> Calculate(DateOnly birthDate)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);

        if (birthDate >= today)
            return Result<PensionResult>.Failure("Fødselsdatoen må ikke være i fremtiden.");

        int age = today.Year - birthDate.Year;
        if (birthDate > today.AddYears(-age))
            age--;

        int yearsUntilPension = Math.Max(0, PensionAge - age);

        string? warning = yearsUntilPension < 5
            ? "Du skal tjekke din pension opsparing."
            : null;

        return Result<PensionResult>.Success(new PensionResult(age, yearsUntilPension, warning));
    }
}