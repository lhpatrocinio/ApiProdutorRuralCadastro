using Microsoft.EntityFrameworkCore;
using ProdutorRuralCadastro.Domain.Entities;
using ProdutorRuralCadastro.Domain.Interfaces;
using ProdutorRuralCadastro.Infrastructure.DataBase.EntityFramework.Context;

namespace ProdutorRuralCadastro.Infrastructure.DataBase.Repository;

public class TalhaoRepository : ITalhaoRepository
{
    private readonly ApplicationDbContext _context;

    public TalhaoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Talhao?> GetByIdAsync(Guid id)
    {
        return await _context.Talhoes.FindAsync(id);
    }

    public async Task<Talhao?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _context.Talhoes
            .Include(t => t.Propriedade)
            .Include(t => t.Cultura)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Talhao>> GetAllAsync()
    {
        return await _context.Talhoes
            .Include(t => t.Cultura)
            .ToListAsync();
    }

    public async Task<IEnumerable<Talhao>> GetAllAtivosAsync()
    {
        return await _context.Talhoes
            .Include(t => t.Cultura)
            .Where(t => t.Ativo)
            .OrderBy(t => t.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<Talhao>> GetByPropriedadeIdAsync(Guid propriedadeId)
    {
        return await _context.Talhoes
            .Include(t => t.Cultura)
            .Where(t => t.PropriedadeId == propriedadeId && t.Ativo)
            .OrderBy(t => t.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<Talhao>> GetByProdutorIdAsync(Guid produtorId)
    {
        return await _context.Talhoes
            .Include(t => t.Propriedade)
            .Include(t => t.Cultura)
            .Where(t => t.Propriedade != null && 
                        t.Propriedade.ProdutorId == produtorId && 
                        t.Ativo)
            .OrderBy(t => t.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<Talhao>> GetByStatusAsync(int status)
    {
        return await _context.Talhoes
            .Include(t => t.Propriedade)
            .Include(t => t.Cultura)
            .Where(t => t.Status == status && t.Ativo)
            .OrderBy(t => t.Nome)
            .ToListAsync();
    }

    public async Task<Talhao> AddAsync(Talhao entity)
    {
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;
        entity.StatusDescricao = entity.GetStatusDescricao();
        await _context.Talhoes.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Talhao entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        entity.StatusDescricao = entity.GetStatusDescricao();
        _context.Talhoes.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(Guid id, int status, string? descricao)
    {
        var entity = await _context.Talhoes.FindAsync(id);
        if (entity != null)
        {
            entity.Status = status;
            entity.StatusDescricao = descricao ?? entity.GetStatusDescricao();
            entity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Talhoes.FindAsync(id);
        if (entity != null)
        {
            entity.Ativo = false;
            entity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
