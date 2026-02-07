using ProdutorRuralCadastro.Domain.Entities;

namespace ProdutorRuralCadastro.Domain.Interfaces;

/// <summary>
/// Interface do repositório de Talhões
/// </summary>
public interface ITalhaoRepository : IRepository<Talhao>
{
    Task<IEnumerable<Talhao>> GetByPropriedadeIdAsync(Guid propriedadeId);
    Task<IEnumerable<Talhao>> GetByProdutorIdAsync(Guid produtorId);
    Task<Talhao?> GetByIdWithDetailsAsync(Guid id);
    Task<IEnumerable<Talhao>> GetByStatusAsync(int status);
    Task<IEnumerable<Talhao>> GetAllAtivosAsync();
    Task UpdateStatusAsync(Guid id, int status, string? descricao);
}
