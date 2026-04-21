using SchoolProject.Enums;
using SchoolProject.Models;

namespace SchoolProject.Services;

/// <summary>
/// Håndterer generering af immutable EmployeeRecord snapshots
/// ud fra en liste af mutable EmployeeDTO objekter.
/// </summary>
public class EmployeeService
{
    private const int PensionAge = 67;
    private const int NearRetirementThreshold = 5;

    /// <summary>
    /// Returnerer en liste af records filtreret på køn.
    /// Parameter: list af medarbejdere + ønsket køn.
    /// Return type: List af EmployeeRecord
    /// </summary>
    public List<EmployeeRecord> GetByGender(List<EmployeeDTO> employees, Gender gender)
    {
        return employees
            .Where(e => e.Køn == gender)
            .Select(e => new EmployeeRecord(e.ForNavn, e.EfterNavn, e.Age, e.Køn, e.Afdeling))
            .ToList();
    }

    /// <summary>
    /// Returnerer en liste af records for medarbejdere med 5 eller færre år til pension.
    /// Pensionsalder = 67 år
    /// </summary>
    public List<EmployeeRecord> GetNearingRetirement(List<EmployeeDTO> employees)
    {
        return employees
            .Where(e => PensionAge - e.Age <= NearRetirementThreshold)
            .Select(e => new EmployeeRecord(e.ForNavn, e.EfterNavn, e.Age, e.Køn, e.Afdeling))
            .ToList();
    }
}
