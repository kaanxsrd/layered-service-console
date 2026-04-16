namespace Project.Services;

public record PensionResult(
    int     Age,
    int     YearsUntilPension,
    string? Warning
);
