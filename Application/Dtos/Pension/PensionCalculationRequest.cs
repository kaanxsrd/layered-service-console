// Input DTO for pension calculation: takes a date of birth.
namespace Project.Application.Dtos.Pension;

/// <summary>Input to the pension calculator.</summary>
/// <param name="BirthDate">The user's date of birth.</param>
public sealed record PensionCalculationRequest(DateOnly BirthDate);
