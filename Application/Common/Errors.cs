// Centralized error code constants and factory methods for building Error values.
namespace Project.Application.Common;

/// <summary>
/// Central catalog of error codes used across the application, together with factory
/// helpers that produce <see cref="Error"/> values.
/// </summary>
public static class Errors
{
    /// <summary>Stable error-code constants used on <see cref="Error.Code"/>.</summary>
    public static class Codes
    {
        /// <summary>The requested resource was not found.</summary>
        public const string NotFound = "NotFound";

        /// <summary>Input failed validation.</summary>
        public const string Validation = "Validation";

        /// <summary>Operation conflicts with current state (e.g. duplicate).</summary>
        public const string Conflict = "Conflict";

        /// <summary>Caller is not authenticated or not authorized.</summary>
        public const string Unauthorized = "Unauthorized";

        /// <summary>Unexpected failure (typically surfaced from a caught exception).</summary>
        public const string Unexpected = "Unexpected";
    }

    /// <summary>Creates a "not found" error with the given message.</summary>
    public static Error NotFound(string message) => new(Codes.NotFound, message);

    /// <summary>Creates a validation error with the given message.</summary>
    public static Error Validation(string message) => new(Codes.Validation, message);

    /// <summary>Creates a conflict error with the given message.</summary>
    public static Error Conflict(string message) => new(Codes.Conflict, message);

    /// <summary>Creates an unauthorized error with the given message.</summary>
    public static Error Unauthorized(string message) => new(Codes.Unauthorized, message);

    /// <summary>Creates an unexpected-failure error with the given message.</summary>
    public static Error Unexpected(string message) => new(Codes.Unexpected, message);
}
