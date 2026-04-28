namespace SchoolProject.Employee;

/// <summary>
/// Struct value type der wrapper en fødselsdato for type-safety.
/// Sikrer at kun gyldige datoer accepteres
/// </summary>
public readonly struct BirthDate : IEquatable<BirthDate>
{
    public DateOnly Value { get; }

    public BirthDate(DateOnly date)
    {
        if (date >= DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("Fødselsdato kan ikke være i fremtiden.");
        Value = date;
    }

    public int Age
    {
        get
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - Value.Year;
            if (today < Value.AddYears(age)) age--;
            return age;
        }
    }

    public bool Equals(BirthDate other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is BirthDate other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString("dd/MM/yyyy");
}
