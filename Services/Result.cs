namespace Project.Services;

/// <summary>
/// Represents the outcome of an operation that either succeeds with a value of type
/// <typeparamref name="T"/> or fails with an error message.
/// </summary>
public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string Error { get; }

    private Result(bool isSuccess, T value, string error)
    {
        IsSuccess = isSuccess;
        Value     = value;
        Error     = error;
    }

    public static Result<T> Success(T value) => new(true, value,null);
    public static Result<T> Failure(string error) => new(false, default, error);
}
