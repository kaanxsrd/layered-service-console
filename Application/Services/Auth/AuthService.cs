// Default IAuthService implementation: uses repository + hasher + token service.
using Microsoft.Extensions.Logging;
using Project.Application.Abstractions;
using Project.Application.Common;
using Project.Application.Dtos.Auth;
using Project.Application.Mapping;
using Project.Domain.Entities;
using Project.Domain.Enums;

namespace Project.Application.Services.Auth;

/// <summary>Default <see cref="IAuthService"/> implementation backed by DI-supplied collaborators.</summary>
public sealed class AuthService : IAuthService
{
    private readonly IUserRepository      _users;
    private readonly IPasswordHasher      _hasher;
    private readonly ITokenService        _tokens;
    private readonly ILogger<AuthService> _logger;

    /// <summary>Creates a new <see cref="AuthService"/>.</summary>
    public AuthService(
        IUserRepository      users,
        IPasswordHasher      hasher,
        ITokenService        tokens,
        ILogger<AuthService> logger)
    {
        _users  = users;
        _hasher = hasher;
        _tokens = tokens;
        _logger = logger;
    }

    /// <inheritdoc />
    public UserResponse? CurrentUser { get; private set; }

    /// <inheritdoc />
    public bool IsLoggedIn => CurrentUser is not null;

    /// <inheritdoc />
    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            return Result<LoginResponse>.Failure(Errors.Validation("Email is required."));
        if (string.IsNullOrEmpty(request.Password))
            return Result<LoginResponse>.Failure(Errors.Validation("Password is required."));

        var user = await _users.GetByEmailAsync(request.Email, cancellationToken).ConfigureAwait(false);

        // Unified error for unknown email + bad password to avoid user enumeration.
        if (user is null || !_hasher.Verify(request.Password, user.PasswordHash))
        {
            _logger.LogInformation("Failed login for {Email}", request.Email);
            return Result<LoginResponse>.Failure(Errors.Unauthorized("Invalid email or password."));
        }

        if (user.Status != Status.Active)
        {
            _logger.LogInformation("Login blocked for inactive account {UserId}", user.Id);
            return Result<LoginResponse>.Failure(Errors.Unauthorized("Account is not active."));
        }

        var token = await _tokens.IssueAsync(user, cancellationToken).ConfigureAwait(false);
        var response = new LoginResponse(user.ToResponse(), token);

        CurrentUser = response.User;
        _logger.LogInformation("User {UserId} logged in", user.Id);

        return Result<LoginResponse>.Success(response);
    }

    /// <inheritdoc />
    public async Task<Result<UserResponse>> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<UserResponse>.Failure(Errors.Validation("Name is required."));
        if (string.IsNullOrWhiteSpace(request.Email))
            return Result<UserResponse>.Failure(Errors.Validation("Email is required."));
        if (string.IsNullOrEmpty(request.Password))
            return Result<UserResponse>.Failure(Errors.Validation("Password is required."));

        if (await _users.ExistsByEmailAsync(request.Email, cancellationToken).ConfigureAwait(false))
            return Result<UserResponse>.Failure(Errors.Conflict("Email is already registered."));

        // TODO(security): enforce password policy + rate-limit login attempts
        var user = new User
        {
            Id           = Guid.NewGuid(),
            Name         = request.Name,
            Email        = request.Email,
            PasswordHash = _hasher.Hash(request.Password),
            Role         = request.Role,
            Status       = Status.Active,
        };

        await _users.AddAsync(user, cancellationToken).ConfigureAwait(false);
        _logger.LogInformation("Registered user {UserId} with role {Role}", user.Id, user.Role);

        return Result<UserResponse>.Success(user.ToResponse());
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<UserResponse>> ListUsersAsync(CancellationToken cancellationToken)
    {
        var users = await _users.GetAllAsync(cancellationToken).ConfigureAwait(false);
        return users.Select(u => u.ToResponse()).ToList();
    }

    /// <inheritdoc />
    public async Task<Result<UserResponse>> SetStatusAsync(Guid targetUserId, Status newStatus, CancellationToken cancellationToken)
    {
        if (CurrentUser is null)
            return Result<UserResponse>.Failure(Errors.Unauthorized("Not logged in."));

        var target = await _users.GetByIdAsync(targetUserId, cancellationToken).ConfigureAwait(false);
        if (target is null)
            return Result<UserResponse>.Failure(Errors.NotFound("User not found."));

        if (target.Role == Role.Root)
            return Result<UserResponse>.Failure(Errors.Unauthorized("The root account cannot be modified."));
        if (target.Id == CurrentUser.Id)
            return Result<UserResponse>.Failure(Errors.Unauthorized("You cannot disable your own account."));
        if (CurrentUser.Role == Role.Admin && target.Role == Role.Admin)
            return Result<UserResponse>.Failure(Errors.Unauthorized("An admin cannot modify another admin."));

        target.Status = newStatus;
        await _users.UpdateAsync(target, cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("User {UserId} status set to {Status}", target.Id, newStatus);
        return Result<UserResponse>.Success(target.ToResponse());
    }

    /// <inheritdoc />
    public void Logout()
    {
        if (CurrentUser is not null)
            _logger.LogInformation("User {UserId} logged out", CurrentUser.Id);
        CurrentUser = null;
    }
}
