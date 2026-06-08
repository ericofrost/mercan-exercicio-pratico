using FluentValidation.Results;

namespace ER.Application.Common;

/// <summary>
/// Represents the outcome of an application operation with a typed payload.
/// </summary>
/// <typeparam name="T">The type of the successful result value.</typeparam>
public class Result<T>
{
    public T? Data { get; set; }

    public bool Success { get; set; }

    public List<Error> Error { get; private set; } = [];
    
    public required ValidationResult Validation { get; set; }

    public static Result<T> Create()
    {
        return new Result<T>
        {
            Data = Activator.CreateInstance<T>(),
            Success = true,
            Validation = new ValidationResult()
        };
    }
    
    public void SetData(T data)
    {
        Data = data;
    }
    
    public void SetError(string errorMessage, ErrorType errorType)
    { 
        Error.Add(new Error(errorMessage, errorType));
        Success = false;
    }
}