using ProdutorRuralCadastro.Application.DTOs.Cultura;

namespace ProdutorRuralCadastro.Application.Services.Interfaces;

public interface ICulturaService
{
    Task<IEnumerable<CulturaResponse>> GetAllAsync();
    Task<CulturaResponse?> GetByIdAsync(Guid id);
}
