using ProdutorRuralCadastro.Domain.Entities;

namespace ProdutorRuralCadastro.Domain.Interfaces;

/// <summary>
/// Interface do repositório de Culturas
/// </summary>
public interface ICulturaRepository : IRepository<Cultura>
{
    Task<IEnumerable<Cultura>> GetAllAtivosAsync();
    Task<Cultura?> GetByNomeAsync(string nome);
}
