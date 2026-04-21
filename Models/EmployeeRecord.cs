using SchoolProject.Enums;

namespace SchoolProject.Models;

/// <summary>
/// Record type – immutable med value-based equality som standard.
/// Bruges som return type / data-snapshot fra EmployeeService.
/// IComparable implementeres for at tillade sortering efter efternavn.
/// </summary>
public record EmployeeRecord(string ForNavn, string EfterNavn, int Alder, Gender Køn, Department Afdeling)
    : IComparable<EmployeeRecord>
{
    /// <summary>Sortering efter efternavn alfabetisk.</summary>
    public int CompareTo(EmployeeRecord? other)
    {
        if (other is null) return 1;
        return string.Compare(EfterNavn, other.EfterNavn, StringComparison.OrdinalIgnoreCase);
    }
}
