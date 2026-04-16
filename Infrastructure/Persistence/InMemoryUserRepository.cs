// In-memory IUserRepository backed by ConcurrentDictionary; seeded with default accounts.
using System.Collections.Concurrent;
using Project.Application.Abstractions;
using Project.Domain.Entities;
using Project.Domain.Enums;

namespace Project.Infrastructure.Persistence;

/// <summary>
/// In-memory <see cref="IUserRepository"/> seeded with the built-in root/admin/user
/// accounts. Intended as the default implementation until persistent storage arrives.
/// </summary>
public sealed class InMemoryUserRepository : IUserRepository
{
    private static readonly Guid RootId  = new("00000000-0000-0000-0000-000000000001");
    private static readonly Guid AdminId = new("00000000-0000-0000-0000-000000000002");
    private static readonly Guid UserId  = new("00000000-0000-0000-0000-000000000003");

    private readonly ConcurrentDictionary<Guid, User> _users;

    /// <summary>Creates a repository seeded with the built-in default accounts.</summary>
    /// <param name="hasher">Password hasher used to hash seed passwords at startup.</param>
    public InMemoryUserRepository(IPasswordHasher hasher)
    {
        ArgumentNullException.ThrowIfNull(hasher);

        _users = new ConcurrentDictionary<Guid, User>();

        Seed(new User
        {
            Id           = RootId,
            Name         = "Root",
            Email        = "root@system.local",
            PasswordHash = hasher.Hash("root123"),
            Role         = Role.Root,
            Status       = Status.Active,
        });
        Seed(new User
        {
            Id           = AdminId,
            Name         = "Admin",
            Email        = "admin@system.local",
            PasswordHash = hasher.Hash("admin123"),
            Role         = Role.Admin,
            Status       = Status.Active,
        });
        Seed(new User
        {
            Id           = UserId,
            Name         = "User",
            Email        = "user@system.local",
            PasswordHash = hasher.Hash("user123"),
            Role         = Role.User,
            Status       = Status.Active,
        });
    }

    /// <inheritdoc />
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        Task.FromResult(_users.TryGetValue(id, out var user) ? user : null);

    /// <inheritdoc />
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var match = _users.Values.FirstOrDefault(
            u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
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
        Task.FromResult(_users.Values.Any(
            u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)));

    private void Seed(User user) => _users[user.Id] = user;
}
