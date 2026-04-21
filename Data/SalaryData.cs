using SchoolProject.Enums;

namespace SchoolProject.Data;

/// <summary>
/// Multidimensionel array: 3 rækker (afdelinger) × 4 kolonner (løn-data).
/// Kolonne-indeks:
///   0 = Gennemsnitsløn kvinde
///   1 = Gennemsnitsløn mand
///   2 = Pension fratrædelsesbonus mand  (mandsløn + 100 %)
///   3 = Pension fratrædelsesbonus kvinde (kvindeløn + 25 %)
/// Række-indeks følger Department enum (SoftwareDevelopment=0, Administration=1, ServiceAndSupport=2).
/// </summary>
public static class SalaryData
{
    public static readonly decimal[,] Løndata =
    {
        //  Kvinde     Mand      Bonus mand          Bonus kvinde
        { 55_000m, 60_000m, 60_000m * 2.00m, 55_000m * 1.25m }, // SoftwareDevelopment
        { 40_000m, 42_000m, 42_000m * 2.00m, 40_000m * 1.25m }, // Administration
        { 38_000m, 41_000m, 41_000m * 2.00m, 38_000m * 1.25m }, // ServiceAndSupport
    };

    public static decimal GetPensionBonus(Department dept, Gender gender)
    {
        int row = (int)dept;
        int col = gender == Gender.M ? 2 : 3;
        return Løndata[row, col];
    }
}
