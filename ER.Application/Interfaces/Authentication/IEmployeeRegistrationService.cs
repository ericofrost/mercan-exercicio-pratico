using ER.Application.Authentication;
using ER.Application.Common;

namespace ER.Application.Interfaces.Authentication;

public interface IEmployeeRegistrationService
{
    Task<Result<RegisterEmployeeResult>> RegisterAsync( RegisterEmployeeRequest request, CancellationToken cancellationToken = default);
}
