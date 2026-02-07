using Microsoft.EntityFrameworkCore;
using ProdutorRuralCadastro.Domain.Entities;
using ProdutorRuralCadastro.Domain.Interfaces;
using ProdutorRuralCadastro.Infrastructure.DataBase.EntityFramework.Context;

namespace ProdutorRuralCadastro.Infrastructure.DataBase.Repository;

public class PropriedadeRepository : IPropriedadeRepository
{
    private readonly ApplicationDbContext _context;

    public PropriedadeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Propriedade?> GetByIdAsync(Guid id)
    {
        return await _context.Propriedades.FindAsync(id);
    }

    public async Task<Propriedade?> GetByIdWithTalhoesAsync(Guid id)
    {
        return await _context.Propriedades
            .Include(p => p.Talhoes)
                .ThenInclude(t => t.Cultura)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Propriedade>> GetAllAsync()
    {
        return await _context.Propriedades.ToListAsync();
    }

    public async Task<IEnumerable<Propriedade>> GetAllAtivosAsync()
    {
        return await _context.Propriedades
            .Where(p => p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<Propriedade>> GetByProdutorIdAsync(Guid produtorId)
    {
        return await _context.Propriedades
            .Where(p => p.ProdutorId == produtorId && p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    public async Task<Propriedade> AddAsync(Propriedade entity)
    {
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;
        await _context.Propriedades.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Propriedade entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Propriedades.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Propriedades.FindAsync(id);
        if (entity != null)
        {
            entity.Ativo = false;
            entity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
