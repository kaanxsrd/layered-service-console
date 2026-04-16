// Unified Result types used across the application layer for success/failure flow.
namespace Project.Application.Common;

/// <summary>
/// Represents the outcome of an operation that either succeeds with a value of
/// type <typeparamref name="T"/> or fails with an <see cref="Common.Error"/>.
/// </summary>
/// <typeparam name="T">The payload type on success.</typeparam>
public sealed class Result<T>
{
    private readonly T? _value;

    private Result(bool isSuccess, T? value, Error? error)
    {
        IsSuccess = isSuccess;
        _value    = value;
        Error     = error;
    }

    /// <summary>True when the operation succeeded and <see cref="Value"/> is available.</summary>
    public bool IsSuccess { get; }

    /// <summary>True when the operation failed and <see cref="Error"/> is populated.</summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// The success value. Accessing this on a failed result throws
    /// <see cref="InvalidOperationException"/>.
    /// </summary>
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access Value on a failed Result.");

    /// <summary>The error describing the failure, or <c>null</c> when <see cref="IsSuccess"/>.</summary>
    public Error? Error { get; }

    /// <summary>Creates a successful result carrying <paramref name="value"/>.</summary>
    public static Result<T> Success(T value) => new(true, value, null);

    /// <summary>Creates a failed result carrying <paramref name="error"/>.</summary>
    public static Result<T> Failure(Error error) => new(false, default, error);
}

/// <summary>
/// Represents the outcome of a void operation that either succeeds or fails with an
/// <see cref="Common.Error"/>.
/// </summary>
public sealed class Result
{
    private Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error     = error;
    }

    /// <summary>True when the operation succeeded.</summary>
    public bool IsSuccess { get; }

    /// <summary>True when the operation failed and <see cref="Error"/> is populated.</summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>The error describing the failure, or <c>null</c> when <see cref="IsSuccess"/>.</summary>
    public Error? Error { get; }

    /// <summary>Creates a successful result.</summary>
    public static Result Success() => new(true, null);

    /// <summary>Creates a failed result carrying <paramref name="error"/>.</summary>
    public static Result Failure(Error error) => new(false, error);
}
