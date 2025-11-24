using BlazorGame.Domain;

namespace BlazorGame.Api.Services;

public interface IDonjonGenerator
{
    Task<Partie> DemarrerPartieAsync(Guid joueurId, CancellationToken cancellationToken = default);
}
