namespace ER.Domain.Base;

/// <summary>
/// Base type for named, activatable domain entities such as <see cref="Models.Tenant"/> and <see cref="Models.Employee"/>.
/// </summary>
public class Model : BaseModel
{
    /// <summary>
    /// Display name of the entity.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Indicates whether the entity is active and may participate in business operations.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Initializes a new instance for Entity Framework Core materialization.
    /// </summary>
    public Model()
    {
    }

    /// <summary>
    /// Initializes a new instance with the required identity and display values.
    /// </summary>
    /// <param name="id">The unique identifier to assign.</param>
    /// <param name="name">The display name.</param>
    /// <param name="isActive">Whether the entity is active. Defaults to <c>true</c>.</param>
    [SetsRequiredMembers]
    public Model(Guid id, string name, bool isActive = true) : base(id)
    {
        Name = name;
        IsActive = isActive;
    }
}
