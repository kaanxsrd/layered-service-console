using SchoolProject.Enums;

namespace SchoolProject.Data;

/// <summary>
/// 3-dimensionel løn-array: [afdeling, køn, løntype]
///
/// Dimension 0 – Afdeling (3 værdier, følger Department enum):
///   0 = SoftwareDevelopment
///   1 = Administration
///   2 = ServiceAndSupport
///
/// Dimension 1 – Køn (2 værdier, følger Gender enum):
///   0 = M  (mand)
///   1 = F  (kvinde)
///
/// Dimension 2 – Løntype (2 værdier, se konstanter nedenfor):
///   0 = Gennemsnitsløn  (AvgWage)
///   1 = Fratrædelsesbonus (PensionBonus)
///
/// Eksempel: Løndata[0, 0, 1]
///   → SoftwareDevelopment, Mand, PensionBonus = 120.000 kr
///
/// Bonus-regler:
///   Mand:   gennemsnitsløn + 100 % → faktor 2,00
///   Kvinde: gennemsnitsløn +  25 % → faktor 1,25
/// </summary>
public static class SalaryData
{
    // --- Indeks-konstanter for Dimension 2 (løntype) ---
    private const int AvgWage    = 0; // Gennemsnitsløn
    private const int Pension    = 1; // Fratrædelsesbonus

    // --- Bonus-faktorer ---
    private const decimal MalePensionFactor   = 2.00m; // løn + 100 %
    private const decimal FemalePensionFactor = 1.25m; // løn +  25 %

    // readonly: array-referencen kan ikke udskiftes,
    // men individuelle celler kan stadig opdateres via UpdateAverageSalary().
    public static readonly decimal[,,] Løndata = new decimal[,,]
    {
        // ── Afdeling 0: SoftwareDevelopment ─────────────────────────────
        {
            // Køn 0 – Mand (M)  →  [0, 0, AvgWage]  [0, 0, Pension]
            { 60_000m,   60_000m * MalePensionFactor   },
            // Køn 1 – Kvinde (F) → [0, 1, AvgWage]  [0, 1, Pension]
            { 55_000m,   55_000m * FemalePensionFactor },
        },

        // ── Afdeling 1: Administration ───────────────────────────────────
        {
            // Køn 0 – Mand
            { 42_000m,   42_000m * MalePensionFactor   },
            // Køn 1 – Kvinde
            { 40_000m,   40_000m * FemalePensionFactor },
        },

        // ── Afdeling 2: ServiceAndSupport ───────────────────────────────
        {
            // Køn 0 – Mand
            { 41_000m,   41_000m * MalePensionFactor   },
            // Køn 1 – Kvinde
            { 38_000m,   38_000m * FemalePensionFactor },
        },
    };

    /// <summary>Returnerer gennemsnitslønnen for den givne afdeling og køn.</summary>
    public static decimal GetAverageSalary(Department dept, Gender gender)
    {
        return Løndata[(int)dept, (int)gender, AvgWage];
    }

    /// <summary>Returnerer fratrædelsesbonus for den givne afdeling og køn.</summary>
    public static decimal GetPensionBonus(Department dept, Gender gender)
    {
        return Løndata[(int)dept, (int)gender, Pension];
    }

    /// <summary>
    /// Opdaterer gennemsnitslønnen og auto-genberegner fratrædelsesbonus.
    /// Mand:   ny bonus = nyLøn * 2,00
    /// Kvinde: ny bonus = nyLøn * 1,25
    /// </summary>
    public static void UpdateAverageSalary(Department dept, Gender gender, decimal newSalary)
    {
        int d = (int)dept;
        int g = (int)gender;

        Løndata[d, g, AvgWage] = newSalary;

        // Auto-opdater fratrædelsesbonus ud fra den nye gennemsnitsløn
        decimal factor = gender == Gender.M ? MalePensionFactor : FemalePensionFactor;
        Løndata[d, g, Pension] = newSalary * factor;
    }
}
