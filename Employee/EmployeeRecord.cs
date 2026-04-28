using SchoolProject.Shared;

namespace SchoolProject.Employee;

/// <summary>
/// Record type – immutable med value-based equality som standard.
/// Bruges som return type / data-snapshot fra EmployeeService.
/// IComparable implementeres for at tillade sortering efter efternavn.
///
/// Alder er et derived attribute (beregnet/afledt data):
/// det opbevares ikke som rådata, men udledes automatisk fra Fødselsdato.
/// </summary>
public record EmployeeRecord(string ForNavn, string EfterNavn, BirthDate Fødselsdato, Gender Køn, Department Afdeling)
    : IComparable<EmployeeRecord>
{
    /// <summary>
    /// Computed property (afledt data) – auto-populeres fra Fødselsdato.
    /// Svarer til en "derived attribute" i UML og et "calculated field" i databaser.
    /// </summary>
    public int Alder => Fødselsdato.Age;

    /// <summary>Sortering efter efternavn alfabetisk.</summary>
    public int CompareTo(EmployeeRecord? other)
    {
        if (other is null) return 1;
        return string.Compare(EfterNavn, other.EfterNavn, StringComparison.OrdinalIgnoreCase);
    }
}
