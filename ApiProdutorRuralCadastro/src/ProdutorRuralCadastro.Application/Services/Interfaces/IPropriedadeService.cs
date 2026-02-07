using ProdutorRuralCadastro.Application.DTOs.Propriedade;

namespace ProdutorRuralCadastro.Application.Services.Interfaces;

public interface IPropriedadeService
{
    Task<IEnumerable<PropriedadeResponse>> GetAllAsync();
    Task<IEnumerable<PropriedadeResponse>> GetByProdutorIdAsync(Guid produtorId);
    Task<PropriedadeResponse?> GetByIdAsync(Guid id);
    Task<PropriedadeComTalhoesResponse?> GetByIdWithTalhoesAsync(Guid id);
    Task<PropriedadeResponse> CreateAsync(PropriedadeCreateRequest request);
    Task<PropriedadeResponse?> UpdateAsync(Guid id, PropriedadeUpdateRequest request);
    Task<bool> DeleteAsync(Guid id);
}
