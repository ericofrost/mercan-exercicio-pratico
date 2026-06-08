using ER.Domain.Enums;
using ER.Domain.Models;

namespace ER.Infrastructure.Seeds.Data;

/// <summary>
/// Predefined sample employees, including two employees and one manager per tenant.
/// </summary>
public static class SampleEmployeeData
{
    /// <summary>
    /// Manager employee for the Acme tenant.
    /// </summary>
    public static readonly Employee AcmeManager = new(
        Guid.Parse("22222222-2222-2222-2222-222222222201"),
        SampleTenantData.Acme.Id,
        "Acme Manager",
        "manager@acme.com",
        EmployeeRole.Manager);

    /// <summary>
    /// First standard employee for the Acme tenant.
    /// </summary>
    public static readonly Employee AcmeEmployee1 = new(
        Guid.Parse("22222222-2222-2222-2222-222222222202"),
        SampleTenantData.Acme.Id,
        "Acme Employee One",
        "employee1@acme.com",
        EmployeeRole.Employee);

    /// <summary>
    /// Second standard employee for the Acme tenant.
    /// </summary>
    public static readonly Employee AcmeEmployee2 = new(
        Guid.Parse("22222222-2222-2222-2222-222222222203"),
        SampleTenantData.Acme.Id,
        "Acme Employee Two",
        "employee2@acme.com",
        EmployeeRole.Employee);

    /// <summary>
    /// Manager employee for the Globex tenant.
    /// </summary>
    public static readonly Employee GlobexManager = new(
        Guid.Parse("22222222-2222-2222-2222-222222222204"),
        SampleTenantData.Globex.Id,
        "Globex Manager",
        "manager@globex.com",
        EmployeeRole.Manager);

    /// <summary>
    /// First standard employee for the Globex tenant.
    /// </summary>
    public static readonly Employee GlobexEmployee1 = new(
        Guid.Parse("22222222-2222-2222-2222-222222222205"),
        SampleTenantData.Globex.Id,
        "Globex Employee One",
        "employee1@globex.com",
        EmployeeRole.Employee);

    /// <summary>
    /// Second standard employee for the Globex tenant.
    /// </summary>
    public static readonly Employee GlobexEmployee2 = new(
        Guid.Parse("22222222-2222-2222-2222-222222222206"),
        SampleTenantData.Globex.Id,
        "Globex Employee Two",
        "employee2@globex.com",
        EmployeeRole.Employee);

    /// <summary>
    /// All predefined sample employees.
    /// </summary>
    public static IReadOnlyList<Employee> All { get; } =
    [
        AcmeManager,
        AcmeEmployee1,
        AcmeEmployee2,
        GlobexManager,
        GlobexEmployee1,
        GlobexEmployee2
    ];
}
