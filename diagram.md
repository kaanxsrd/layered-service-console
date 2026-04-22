# Del 4: Diagrammer

Nedenfor er tre UML-relaterede diagrammer for projektet.

| Diagram | Viser | Fokus |
|---|---|---|
| Domain model | Begreber | Hvad er systemet? |
| Klassediagram | Struktur | Hvordan er det bygget? |
| Flowdiagram | Proces | Hvad sker der? |

## 1) Domain model

```mermaid
classDiagram
    direction LR

    class Medarbejder {}

    class Foedselsdato {}

    class Koen {}

    class Afdeling {}

    class Pension { }

    class Loendata {}

```

## 2) Klassediagram

```mermaid
classDiagram
    direction TB

    class Program {
      +Main()
    }

    class EmployeeDTO {
      +string ForNavn
      +string EfterNavn
      +BirthDate Foedselsdato
      +Gender Koen
      +Department Afdeling
      +int Age
      +CompareTo(other)
      +Equals(other)
      +GetHashCode()
    }

    class EmployeeRecord {
      <<record>>
      +string ForNavn
      +string EfterNavn
      +BirthDate Foedselsdato
      +Gender Koen
      +Department Afdeling
      +int Alder
      +CompareTo(other)
    }

    class BirthDate {
      <<struct>>
      +DateOnly Value
      +int Age
      +ToString()
      +Equals(other)
    }

    class EmployeeService {
      -const int PensionAge = 67
      -const int NearRetirementThreshold = 5
      +GetByGender(employees, gender) List~EmployeeRecord~
      +GetNearingRetirement(employees) List~EmployeeRecord~
    }

    class SeedData {
      <<static>>
      +GetEmployees() List~EmployeeDTO~
    }

    class SalaryData {
      <<static>>
      +decimal[,,] Loendata
      +GetAverageSalary(dept, gender) decimal
      +GetPensionBonus(dept, gender) decimal
      +UpdateAverageSalary(dept, gender, newSalary) void
    }

    class ConsoleHelper {
      <<static>>
      +PrintMenu()
      +PrintDtoList(list)
      +PrintRecordList(list)
      +PrintNearRetirementList(list)
      +PrintSalaryData()
      +UpdateSalaryMatrix()
      +ReadEmployeeFromInput() EmployeeDTO
    }

    class Department {
      <<enum>>
      SoftwareDevelopment
      Administration
      ServiceAndSupport
    }

    class Gender {
      <<enum>>
      M
      F
    }

    Program --> SeedData : henter startdata
    Program --> EmployeeService : bruger
    Program --> ConsoleHelper : viser UI
    Program --> EmployeeDTO : arbejder paa liste

    EmployeeDTO --> BirthDate
    EmployeeDTO --> Department
    EmployeeDTO --> Gender

    EmployeeRecord --> BirthDate
    EmployeeRecord --> Department
    EmployeeRecord --> Gender

    EmployeeService ..> EmployeeDTO : laeser
    EmployeeService ..> EmployeeRecord : opretter snapshots

    ConsoleHelper ..> EmployeeDTO : viser/indlaeser
    ConsoleHelper ..> EmployeeRecord : viser
    ConsoleHelper ..> SalaryData : loendata
    ConsoleHelper ..> Department
    ConsoleHelper ..> Gender

    SeedData ..> EmployeeDTO : opretter
    SeedData ..> BirthDate
    SeedData ..> Department
    SeedData ..> Gender

    SalaryData ..> Department
    SalaryData ..> Gender
```

## 3) Flowdiagram (menu/proces)

```mermaid
flowchart TD
    A[Start program] --> B[Indlaes medarbejdere fra SeedData]
    B --> C[Sorter medarbejdere]
    C --> D[Vis menu]
    D --> E{Brugerens valg}

    E -->|1| F[Vis alle medarbejdere]
    E -->|2| G[Filtrer kvinder via EmployeeService]
    E -->|3| H[Filtrer maend via EmployeeService]
    E -->|4| I[Find <= 5 aar til pension]
    E -->|5| J[Indlaes ny medarbejder]
    E -->|6| K[Vis loendata]
    E -->|7| L[Opdater loenmatrix]
    E -->|8| M[Afslut]
    E -->|Andet| N[Vis fejl: ugyldigt valg]

    F --> O[Pause]
    G --> O
    H --> O
    I --> O
    J --> P{Gyldig og unik?}
    P -->|Ja| Q[Tilfoej og sorter]
    P -->|Nej| R[Vis fejl]
    Q --> O
    R --> O
    K --> O
    L --> O
    N --> O
    O --> D
```

## Noter til aflevering

- Diagrammerne er skrevet i Mermaid og kan vises direkte i Markdown-viewer med Mermaid-support.
- Hvis du vil bruge Umletino, kan du genoptegne samme klasser/relationer manuelt ud fra ovenstående.
