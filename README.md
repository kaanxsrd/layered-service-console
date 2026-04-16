# SchoolProject

A small .NET 8 console application used as a sandbox for role-based auth, a pluggable
service-module system, and a couple of feature services (pension calculator, word
counter) behind a clean layered architecture.

## Features

- Role-based login with three seeded accounts (Root, Admin, User).
- Pluggable **service modules**: any class implementing `IServiceModule` registered with
  DI shows up in the dashboard automatically.
- Built-in modules:
  - **Pension Calculator** — calculates age and years until Danish state pension.
  - **Word Count** — counts a word in a block of pasted text and saves a log file.
- Admin-only user management: create users, toggle status, role-aware rules.

## Architecture

Single project, four internal layers. Dependencies point inward only.

```
Domain          ← entities, enums, value objects (no deps)
Application     ← abstractions, DTOs, business services, Result/Error types
Infrastructure  ← persistence, hashing, clock, token service
Presentation    ← console screens, input helpers, module adapters
Program.cs      ← composition root (DI + main loop)
```

Key contracts: `IAuthService`, `IUserRepository`, `IPasswordHasher`, `ITokenService`,
`IClock`, `IServiceRegistry`, `IServiceModule`, `IScreen`.

Results flow as `Result<T>` / `Result` with an `Error(Code, Message)`; no exceptions for
expected failures. All console output is rendered through `ResultRenderer`.

## Getting started

Requires the .NET 8 SDK (or newer — `RollForward=LatestMajor` is set).

```bash
dotnet build
dotnet run --project Project.csproj
```

Dev shortcuts:

```bash
dotnet run -- --dev    # auto-login as root
dotnet run -- --user   # auto-login as standard user
```

### Default accounts

| Email                 | Password  | Role  |
|-----------------------|-----------|-------|
| `root@system.local`   | `root123` | Root  |
| `admin@system.local`  | `admin123`| Admin |
| `user@system.local`   | `user123` | User  |

Passwords are hashed on startup with PBKDF2-SHA256 (100k iterations, per-user salt).

## TODO markers

Grep the repo for `TODO(...)` to find pending work:

- `TODO(db)` — swap `InMemoryUserRepository` for `SqlUserRepository` + EF Core.
- `TODO(auth)` — replace `StubTokenService` with a real token/session implementation.
- `TODO(config)` — move secrets to `IConfiguration` / user-secrets.
- `TODO(security)` — enforce password policy and rate-limit login attempts.

## Adding a new service module

1. Create a class implementing `IServiceModule` (or a thin adapter in `Presentation/Modules/`
   wrapping a screen).
2. Register it in `Program.cs`:
   ```csharp
   services.AddSingleton<IServiceModule, MyNewModule>();
   ```
3. It shows up in the dashboard automatically, filtered by `MinimumRole`.
