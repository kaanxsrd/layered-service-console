// In-memory IUserRepository backed by ConcurrentDictionary; seeded from appsettings.json.
using System.Collections.Concurrent;
using Project.Application.Abstractions;
using Project.Domain.Entities;
using Project.Domain.Enums;
using Project.Domain.ValueObjects;

namespace Project.Infrastructure.Persistence;

/// <summary>
/// In-memory <see cref="IUserRepository"/> whose seed users are loaded from the
/// <c>Users</c> array in <c>appsettings.json</c>. Add, edit, or remove entries there
/// to change who exists at startup without recompiling.
/// Falls back to three built-in accounts when the config key is absent.
/// </summary>
public sealed class InMemoryUserRepository : IUserRepository
{
    private static readonly IReadOnlyList<UserSeedEntry> FallbackSeed =
    [
        new(new Guid("00000000-0000-0000-0000-000000000001"), "Root",  "root@system.local",  "root123",  Role.Root,  Status.Active),
        new(new Guid("00000000-0000-0000-0000-000000000002"), "Admin", "admin@system.local", "admin123", Role.Admin, Status.Active),
        new(new Guid("00000000-0000-0000-0000-000000000003"), "User",  "user@system.local",  "user123",  Role.User,  Status.Active),
    ];

    private readonly ConcurrentDictionary<Guid, User> _users;

    /// <summary>Creates a repository seeded from <paramref name="config"/> or the built-in defaults.</summary>
    public InMemoryUserRepository(IPasswordHasher hasher, IConfigurationService config)
    {
        ArgumentNullException.ThrowIfNull(hasher);
        ArgumentNullException.ThrowIfNull(config);

        _users = new ConcurrentDictionary<Guid, User>();

        var seedResult = config.Get<List<UserSeedEntry>>("Users");
        var entries    = seedResult.IsSuccess && seedResult.Value.Count > 0
            ? seedResult.Value
            : (IReadOnlyList<UserSeedEntry>)FallbackSeed;

        foreach (var entry in entries)
        {
            _users[entry.Id] = new User
            {
                Id           = entry.Id,
                Name         = entry.Name,
                Email        = Email.From(entry.Email),
                PasswordHash = hasher.Hash(entry.Password),
                Role         = entry.Role,
                Status       = entry.Status,
            };
        }
    }

    /// <inheritdoc />
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        Task.FromResult(_users.TryGetValue(id, out var user) ? user : null);

    /// <inheritdoc />
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var match = _users.Values.FirstOrDefault(u => u.Email.Equals(email));
        return Task.FromResult<User?>(match);
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken) =>
        Task.FromResult<IReadOnlyList<User>>(_users.Values.ToList());

    /// <inheritdoc />
    public Task AddAsync(User user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        if (!_users.TryAdd(user.Id, user))
            throw new InvalidOperationException($"User {user.Id} already exists.");
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        _users[user.Id] = user;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken) =>
        Task.FromResult(_users.Values.Any(u => u.Email.Equals(email)));
}
