namespace Project.Services;

/// <summary>
/// DTO returned by <see cref="PensionCalculator.Calculate"/>.
/// </summary>

public record PensionResult(
    int     Age,
    int     YearsUntilPension,
    string? Warning
);
