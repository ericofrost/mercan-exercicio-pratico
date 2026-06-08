using ER.Domain.Shared;

namespace ER.Application.Interfaces.Authentication;

public interface ITokenGeneratorService
{
    ValueTask<string?> GenerateTokenAsync(GenerateTokenRequest request);
}