using ProdutorRuralCadastro.Application.DTOs.Talhao;

namespace ProdutorRuralCadastro.Application.Services.Interfaces;

public interface ITalhaoService
{
    Task<IEnumerable<TalhaoResponse>> GetAllAsync();
    Task<IEnumerable<TalhaoResponse>> GetByPropriedadeIdAsync(Guid propriedadeId);
    Task<IEnumerable<TalhaoComDetalhesResponse>> GetByProdutorIdAsync(Guid produtorId);
    Task<TalhaoResponse?> GetByIdAsync(Guid id);
    Task<TalhaoComDetalhesResponse?> GetByIdWithDetailsAsync(Guid id);
    Task<TalhaoResponse> CreateAsync(TalhaoCreateRequest request);
    Task<TalhaoResponse?> UpdateAsync(Guid id, TalhaoUpdateRequest request);
    Task<bool> UpdateStatusAsync(Guid id, TalhaoUpdateStatusRequest request);
    Task<bool> DeleteAsync(Guid id);
}
