using ProdutorRuralCadastro.Domain.Entities;

namespace ProdutorRuralCadastro.Domain.Interfaces;

/// <summary>
/// Interface do repositório de Propriedades
/// </summary>
public interface IPropriedadeRepository : IRepository<Propriedade>
{
    Task<IEnumerable<Propriedade>> GetByProdutorIdAsync(Guid produtorId);
    Task<Propriedade?> GetByIdWithTalhoesAsync(Guid id);
    Task<IEnumerable<Propriedade>> GetAllAtivosAsync();
}
