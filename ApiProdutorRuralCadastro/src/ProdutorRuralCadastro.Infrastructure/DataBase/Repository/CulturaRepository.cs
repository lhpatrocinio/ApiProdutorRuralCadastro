using Microsoft.EntityFrameworkCore;
using ProdutorRuralCadastro.Domain.Entities;
using ProdutorRuralCadastro.Domain.Interfaces;
using ProdutorRuralCadastro.Infrastructure.DataBase.EntityFramework.Context;

namespace ProdutorRuralCadastro.Infrastructure.DataBase.Repository;

public class CulturaRepository : ICulturaRepository
{
    private readonly ApplicationDbContext _context;

    public CulturaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Cultura?> GetByIdAsync(Guid id)
    {
        return await _context.Culturas.FindAsync(id);
    }

    public async Task<IEnumerable<Cultura>> GetAllAsync()
    {
        return await _context.Culturas.ToListAsync();
    }

    public async Task<IEnumerable<Cultura>> GetAllAtivosAsync()
    {
        return await _context.Culturas
            .Where(c => c.Ativo)
            .OrderBy(c => c.Nome)
            .ToListAsync();
    }

    public async Task<Cultura?> GetByNomeAsync(string nome)
    {
        return await _context.Culturas
            .FirstOrDefaultAsync(c => c.Nome.ToLower() == nome.ToLower());
    }

    public async Task<Cultura> AddAsync(Cultura entity)
    {
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;
        await _context.Culturas.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Cultura entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Culturas.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Culturas.FindAsync(id);
        if (entity != null)
        {
            _context.Culturas.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
