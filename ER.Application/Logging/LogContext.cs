namespace ER.Application.Logging;

/// <summary>
/// Identifies the caller type and method for centralized structured logging.
/// </summary>
/// <param name="CallerType">The name of the calling type.</param>
/// <param name="CallerMethod">The name of the calling method.</param>
public readonly record struct LogContext(string CallerType, string CallerMethod)
{
    /// <summary>
    /// Creates a log context for the specified type and current member.
    /// </summary>
    public static LogContext For<T>([CallerMemberName] string method = "")
        => new(typeof(T).Name, method);
}
