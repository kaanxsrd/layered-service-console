using SchoolProject.Shared;

namespace SchoolProject.Employee;

/// <summary>
/// DTO mutable reference type.
/// Standard i C# er IKKE value-based equality, derfor implementeres IEquatable manuelt.
/// IComparable bruges til at definere sorteringsrækkefølge
/// </summary>
public class EmployeeDTO : IComparable<EmployeeDTO>, IEquatable<EmployeeDTO>
{
    public string ForNavn { get; set; }
    public string EfterNavn { get; set; }
    public BirthDate Fødselsdato { get; set; }
    public Gender Køn { get; set; }
    public Department Afdeling { get; set; }

    public EmployeeDTO(string forNavn, string efterNavn, BirthDate fødselsdato, Gender køn, Department afdeling)
    {
        ForNavn = forNavn;
        EfterNavn = efterNavn;
        Fødselsdato = fødselsdato;
        Køn = køn;
        Afdeling = afdeling;
    }

    public int Age => Fødselsdato.Age;

    /// <summary>
    /// IComparable: definerer sortering efter efternavn (alfabetisk).
    /// Bruges af List.Sort() og lignende.
    /// </summary>
    public int CompareTo(EmployeeDTO? other)
    {
        if (other is null) return 1;
        return string.Compare(EfterNavn, other.EfterNavn, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// IEquatable: value-based equality – sammenligner objektet som helhed.
    /// Bemærk: vi sammenligner det ene objekt med det andet, IKKE felt for felt direkte.
    /// </summary>
    public bool Equals(EmployeeDTO? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(ForNavn, other.ForNavn, StringComparison.OrdinalIgnoreCase)
            && string.Equals(EfterNavn, other.EfterNavn, StringComparison.OrdinalIgnoreCase)
            && Fødselsdato.Equals(other.Fødselsdato)
            && Køn == other.Køn
            && Afdeling == other.Afdeling;
    }

    public override bool Equals(object? obj) => Equals(obj as EmployeeDTO);

    public override int GetHashCode() => HashCode.Combine(
        ForNavn.ToLowerInvariant(),
        EfterNavn.ToLowerInvariant(),
        Fødselsdato,
        Køn,
        Afdeling);
}
