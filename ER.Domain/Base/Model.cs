namespace ER.Domain.Base;

/// <summary>
/// Base class for all domain models with common properties.
/// </summary>
public class Model : BaseModel
{
    /// <summary>
    /// The name
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Marks the record as active or not. It could be an employee or tenant.
    /// </summary>
    public bool IsActive { get; set; } = true;
}