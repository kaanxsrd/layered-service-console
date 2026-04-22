using SchoolProject.Data;
using SchoolProject.Enums;
using SchoolProject.Models;
using SchoolProject.Services;
using SchoolProject.UI;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var employees = SeedData.GetEmployees();
employees.Sort();
var service = new EmployeeService();

bool running = true;

while (running)
{
    Console.Clear();
    ConsoleHelper.Header("Medarbejder Pension Oversigt");
    ConsoleHelper.PrintMenu();

    var choice = Console.ReadLine()?.Trim() ?? "";

    switch (choice)
    {
        case "1":
            Console.Clear();
            ConsoleHelper.Header("Medarbejder Pension Oversigt");
            ConsoleHelper.Section("Alle medarbejdere");
            ConsoleHelper.PrintDtoList(employees);
            ConsoleHelper.Pause();
            break;

        case "2":
            Console.Clear();
            ConsoleHelper.Header("Medarbejder Pension Oversigt");
            ConsoleHelper.Section("Kvindelige medarbejdere");
            var female = service.GetByGender(employees, Gender.F);
            female.Sort();
            ConsoleHelper.PrintRecordList(female);
            ConsoleHelper.Pause();
            break;

        case "3":
            Console.Clear();
            ConsoleHelper.Header("Medarbejder Pension Oversigt");
            ConsoleHelper.Section("Mandlige medarbejdere");
            var male = service.GetByGender(employees, Gender.M);
            male.Sort();
            ConsoleHelper.PrintRecordList(male);
            ConsoleHelper.Pause();
            break;

        case "4":
            Console.Clear();
            ConsoleHelper.Header("Medarbejder Pension Oversigt");
            ConsoleHelper.Section("Medarbejdere med 5 eller færre år til pension");
            var nearPension = service.GetNearingRetirement(employees);
            nearPension.Sort();
            ConsoleHelper.PrintNearRetirementList(nearPension);
            ConsoleHelper.Pause();
            break;

        case "5":
            Console.Clear();
            ConsoleHelper.Header("Medarbejder Pension Oversigt");
            ConsoleHelper.Section("Registrere ny medarbejder");
            var newEmployee = ConsoleHelper.ReadEmployeeFromInput();
            if (newEmployee is not null)
            {
                if (employees.Any(e => e.Equals(newEmployee)))
                    ConsoleHelper.Error("Medarbejderen eksisterer allerede i listen.");
                else
                {
                    employees.Add(newEmployee);
                    employees.Sort();
                    ConsoleHelper.Success("Medarbejder tilføjet!");
                    ConsoleHelper.PrintDtoList(employees);
                }
            }
            ConsoleHelper.Pause();
            break;

        case "6":
            Console.Clear();
            ConsoleHelper.Header("Medarbejder Pension Oversigt");
            ConsoleHelper.Section("Løndata oversigt");
            ConsoleHelper.PrintSalaryData();
            ConsoleHelper.Pause();
            break;

        case "7":
            Console.Clear();
            ConsoleHelper.Header("Medarbejder Pension Oversigt");
            ConsoleHelper.Section("Opdater løn matrix");
            ConsoleHelper.UpdateSalaryMatrix();
            ConsoleHelper.Pause();
            break;

        case "8":
            running = false;
            break;

        default:
            ConsoleHelper.Error("Ugyldigt valg – vælg venligst 1-8.");
            ConsoleHelper.Pause();
            break;
    }
}
