using SchoolProject.Data;
using SchoolProject.Enums;
using SchoolProject.Models;
using SchoolProject.Services;
using SchoolProject.UI;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var employees = SeedData.GetEmployees();
var service = new EmployeeService();

ConsoleHelper.Header("Medarbejder Pension Oversigt");

// del 1 vis alle medarbejdere med alder
ConsoleHelper.Section("Øvelse Del 1 – Medarbejderliste");
ConsoleHelper.PrintDtoList(employees);

// del 2 sorter listen alfabetisk efter efternavn og lad brugeren tilføje en ny
ConsoleHelper.Section("Øvelse Del 2 – Sorteret efter efternavn");
employees.Sort();
ConsoleHelper.PrintDtoList(employees);

ConsoleHelper.Section("Tilføj ny medarbejder");
var newEmployee = ConsoleHelper.ReadEmployeeFromInput();

if (newEmployee is not null)
{
    // tjek om medarbejderen allerede findes via IEquatable
    if (employees.Any(e => e.Equals(newEmployee)))
    {
        ConsoleHelper.Error("Medarbejderen eksisterer allerede i listen.");
    }
    else
    {
        employees.Add(newEmployee);
        employees.Sort();
        ConsoleHelper.Success("Medarbejder tilføjet!");
        ConsoleHelper.PrintDtoList(employees);
    }
}

// del 3 og 4 konverter til records og filtrer på køn og pensionsalder
ConsoleHelper.Section("Øvelse Del 3 & 4 – Medarbejder Records");

var femaleRecords = service.GetByGender(employees, Gender.F);
var maleRecords   = service.GetByGender(employees, Gender.M);
var nearPension   = service.GetNearingRetirement(employees);

Console.WriteLine("  Kvindelige medarbejdere:");
ConsoleHelper.PrintRecordList(femaleRecords);

Console.WriteLine("\n  Mandlige medarbejdere:");
ConsoleHelper.PrintRecordList(maleRecords);

Console.WriteLine("\n  Medarbejdere inden for 5 år til pension (folkepensionsalder: 67):");
ConsoleHelper.PrintRecordList(nearPension);

// del 5 sorter alle record-lister alfabetisk efter efternavn
ConsoleHelper.Section("Øvelse Del 5 – Records sorteret efter efternavn");

femaleRecords.Sort();
maleRecords.Sort();
nearPension.Sort();

Console.WriteLine("  Kvindelige medarbejdere (sorteret):");
ConsoleHelper.PrintRecordList(femaleRecords);

Console.WriteLine("\n  Mandlige medarbejdere (sorteret):");
ConsoleHelper.PrintRecordList(maleRecords);

Console.WriteLine("\n  Medarbejdere inden for 5 år til pension (sorteret):");
ConsoleHelper.PrintRecordList(nearPension);

Console.WriteLine("\n  Tryk på en tast for at afslutte...");
Console.ReadKey(intercept: true);
