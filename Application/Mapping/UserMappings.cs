// Explicit mappings between User aggregate and auth DTOs.
using Project.Application.Dtos.Auth;
using Project.Domain.Entities;

namespace Project.Application.Mapping;

/// <summary>Explicit, hand-written mappings for <see cref="User"/>.</summary>
public static class UserMappings
{
    /// <summary>Projects a <see cref="User"/> entity to a public <see cref="UserResponse"/> DTO.</summary>
    /// <param name="user">The source user.</param>
    public static UserResponse ToResponse(this User user) =>
        new(user.Id, user.Name, user.Email.Value, user.Role, user.Status);
}
