namespace ER.Application.Common;

/// <summary>
/// Represents the outcome of an application operation without a typed payload.
/// </summary>
public class Result
{
    /// <summary>
    /// Initializes a new result instance.
    /// </summary>
    /// <param name="isSuccess">Whether the operation succeeded.</param>
    /// <param name="error">The error message when the operation failed.</param>
    protected Result(bool isSuccess, string? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Indicates whether the operation completed successfully.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Indicates whether the operation failed.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Error message describing the failure. <c>null</c> when <see cref="IsSuccess"/> is <c>true</c>.
    /// </summary>
    public string? Error { get; }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <returns>A successful <see cref="Result"/> instance.</returns>
    public static Result Success() => new(true, null);

    /// <summary>
    /// Creates a failed result with the supplied error message.
    /// </summary>
    /// <param name="error">The failure message.</param>
    /// <returns>A failed <see cref="Result"/> instance.</returns>
    public static Result Failure(string error) => new(false, error);
}

/// <summary>
/// Represents the outcome of an application operation with a typed payload.
/// </summary>
/// <typeparam name="T">The type of the successful result value.</typeparam>
public sealed class Result<T> : Result
{
    /// <summary>
    /// Initializes a new typed result instance.
    /// </summary>
    /// <param name="isSuccess">Whether the operation succeeded.</param>
    /// <param name="value">The successful payload.</param>
    /// <param name="error">The error message when the operation failed.</param>
    private Result(bool isSuccess, T? value, string? error) : base(isSuccess, error)
    {
        Value = value;
    }

    /// <summary>
    /// Successful payload. <c>null</c> when <see cref="Result.IsSuccess"/> is <c>false</c>.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Creates a successful typed result.
    /// </summary>
    /// <param name="value">The successful payload.</param>
    /// <returns>A successful <see cref="Result{T}"/> instance.</returns>
    public static Result<T> Success(T value) => new(true, value, null);

    /// <summary>
    /// Creates a failed typed result with the supplied error message.
    /// </summary>
    /// <param name="error">The failure message.</param>
    /// <returns>A failed <see cref="Result{T}"/> instance.</returns>
    public static new Result<T> Failure(string error) => new(false, default, error);
}
