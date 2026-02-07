using ProdutorRuralCadastro.Domain.Entities;

namespace ProdutorRuralCadastro.Domain.Interfaces;

/// <summary>
/// Interface base para repositórios
/// </summary>
public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}
