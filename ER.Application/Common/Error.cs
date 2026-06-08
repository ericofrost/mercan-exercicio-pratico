namespace ER.Application.Common;

/// <summary>
/// Represents a single application error with a message and classification.
/// </summary>
/// <param name="ErrorMessage">The human-readable error description.</param>
/// <param name="ErrorType">The category of the error.</param>
public record Error(string ErrorMessage, ErrorType ErrorType);