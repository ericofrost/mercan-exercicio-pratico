namespace ER.Application.Common;

/// <summary>
/// Categorizes errors returned in <see cref="Result{T}"/>.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// Expected business or domain failure handled by the service.
    /// </summary>
    Service,

    /// <summary>
    /// Unexpected failure caused by an exception during processing.
    /// </summary>
    Exception
}