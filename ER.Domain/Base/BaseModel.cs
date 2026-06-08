namespace ER.Domain.Base;

/// <summary>
/// Root base type for all persisted domain entities identified by a <see cref="Guid"/> primary key.
/// </summary>
public class BaseModel
{
    /// <summary>
    /// Unique identifier of the entity.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance for Entity Framework Core materialization.
    /// </summary>
    public BaseModel()
    {
    }

    /// <summary>
    /// Initializes a new instance with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier to assign.</param>
    public BaseModel(Guid id) => Id = id;
}
