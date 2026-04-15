using Project.Enums;
using Project.Models;

namespace Project.Data;

/// <summary>
/// Static data access repository for users.
/// </summary>
public static class UserRepository
{
    private static readonly List<User> _users = new()
    {
        new User { Id = 1, Name = "Root",  Email = "root@system.local",  Password = "root123",  Role = Role.Root,  Status = Status.Active },
        new User { Id = 2, Name = "Admin", Email = "admin@system.local", Password = "admin123", Role = Role.Admin, Status = Status.Active },
        new User { Id = 3, Name = "User",  Email = "user@system.local",  Password = "user123",  Role = Role.User,  Status = Status.Active },
    };

    private static int _nextId = 4;

    /// <summary>
    /// Retrieves all registered users in the system.
    /// </summary>
    /// <returns>List of all users.</returns>
    public static List<User> GetAll() => _users.ToList();

    /// <summary>
    /// Finds a user by their email address.
    /// </summary>
    /// <param name="email">The email to search for.</param>
    /// <returns>The matching user or null if not found.</returns>
    public static User? GetByEmail(string email) =>
        _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Finds a user by their unique ID.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>The matching user or null if not found.</returns>
    public static User? GetById(int id) =>
        _users.FirstOrDefault(u => u.Id == id);

    /// <summary>
    /// Adds a new user to the repository and assigns an ID.
    /// </summary>
    /// <param name="user">The user object to add.</param>
    public static void Add(User user)
    {
        user.Id = _nextId++;
        _users.Add(user);
    }

    /// <summary>
    /// Updates an existing user record based on the user ID.
    /// </summary>
    /// <param name="updated">The user object containing updated information.</param>
    public static void Update(User updated)
    {
        int i = _users.FindIndex(u => u.Id == updated.Id);
        if (i >= 0) _users[i] = updated;
    }
}
