using SchoolProject.Enums;
using SchoolProject.Models;

namespace SchoolProject.Data;

/// <summary>
/// Testdata: 10 medarbejdere – 6 kvinder (F), 4 mænd (M).
/// </summary>
public static class SeedData
{
    public static List<EmployeeDTO> GetEmployees() =>
    [
        // 6 kvinder (F)
        new("Anna",      "Jensen",      new BirthDate(new DateOnly(1990,  3, 15)), Gender.F),
        new("Maria",     "Nielsen",     new BirthDate(new DateOnly(1985,  7, 22)), Gender.F),
        new("Sofia",     "Andersen",     new BirthDate(new DateOnly(1992, 11,  8)), Gender.F),
        new("Laura",     "Christensen", new BirthDate(new DateOnly(1959,  4, 30)), Gender.F), // 1 år til pension
        new("Mette",     "Pedersen",    new BirthDate(new DateOnly(1962,  9, 12)), Gender.F), // 4 år til pension
        new("Julie",     "Hansen",      new BirthDate(new DateOnly(1995,  6, 18)), Gender.F),

        // 4 mænd (M)
        new("Peter",     "Sørensen",    new BirthDate(new DateOnly(1988,  2, 14)), Gender.M),
        new("Mikkel",    "Larsen",      new BirthDate(new DateOnly(1975, 12,  3)), Gender.M),
        new("Thomas",    "Møller",      new BirthDate(new DateOnly(1982,  8, 27)), Gender.M),
        new("Christian", "Johansen",    new BirthDate(new DateOnly(2000,  5, 11)), Gender.M),
    ];
}
