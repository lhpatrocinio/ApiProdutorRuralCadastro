using ProdutorRuralCadastro.Application.DTOs.Propriedade;
using ProdutorRuralCadastro.Application.DTOs.Talhao;
using ProdutorRuralCadastro.Application.Services.Interfaces;
using ProdutorRuralCadastro.Domain.Entities;
using ProdutorRuralCadastro.Domain.Interfaces;

namespace ProdutorRuralCadastro.Application.Services;

public class PropriedadeService : IPropriedadeService
{
    private readonly IPropriedadeRepository _propriedadeRepository;

    public PropriedadeService(IPropriedadeRepository propriedadeRepository)
    {
        _propriedadeRepository = propriedadeRepository;
    }

    public async Task<IEnumerable<PropriedadeResponse>> GetAllAsync()
    {
        var propriedades = await _propriedadeRepository.GetAllAtivosAsync();
        return propriedades.Select(MapToResponse);
    }

    public async Task<IEnumerable<PropriedadeResponse>> GetByProdutorIdAsync(Guid produtorId)
    {
        var propriedades = await _propriedadeRepository.GetByProdutorIdAsync(produtorId);
        return propriedades.Select(MapToResponse);
    }

    public async Task<PropriedadeResponse?> GetByIdAsync(Guid id)
    {
        var propriedade = await _propriedadeRepository.GetByIdAsync(id);
        return propriedade == null ? null : MapToResponse(propriedade);
    }

    public async Task<PropriedadeComTalhoesResponse?> GetByIdWithTalhoesAsync(Guid id)
    {
        var propriedade = await _propriedadeRepository.GetByIdWithTalhoesAsync(id);
        if (propriedade == null) return null;

        return new PropriedadeComTalhoesResponse(
            propriedade.Id,
            propriedade.ProdutorId,
            propriedade.Nome,
            propriedade.Endereco,
            propriedade.Cidade,
            propriedade.Estado,
            propriedade.CEP,
            propriedade.Latitude,
            propriedade.Longitude,
            propriedade.AreaTotalHa,
            propriedade.Ativo,
            propriedade.CreatedAt,
            propriedade.UpdatedAt,
            propriedade.Talhoes.Where(t => t.Ativo).Select(t => new TalhaoResumoResponse(
                t.Id,
                t.Nome,
                t.Codigo,
                t.AreaHa,
                t.Status,
                t.StatusDescricao ?? t.GetStatusDescricao(),
                t.Cultura?.Nome
            ))
        );
    }

    public async Task<PropriedadeResponse> CreateAsync(PropriedadeCreateRequest request)
    {
        var propriedade = new Propriedade
        {
            ProdutorId = request.ProdutorId,
            Nome = request.Nome,
            Endereco = request.Endereco,
            Cidade = request.Cidade,
            Estado = request.Estado,
            CEP = request.CEP,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            AreaTotalHa = request.AreaTotalHa,
            Ativo = true
        };

        var created = await _propriedadeRepository.AddAsync(propriedade);
        return MapToResponse(created);
    }

    public async Task<PropriedadeResponse?> UpdateAsync(Guid id, PropriedadeUpdateRequest request)
    {
        var propriedade = await _propriedadeRepository.GetByIdAsync(id);
        if (propriedade == null) return null;

        propriedade.Nome = request.Nome;
        propriedade.Endereco = request.Endereco;
        propriedade.Cidade = request.Cidade;
        propriedade.Estado = request.Estado;
        propriedade.CEP = request.CEP;
        propriedade.Latitude = request.Latitude;
        propriedade.Longitude = request.Longitude;
        propriedade.AreaTotalHa = request.AreaTotalHa;
        propriedade.Ativo = request.Ativo;

        await _propriedadeRepository.UpdateAsync(propriedade);
        return MapToResponse(propriedade);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var propriedade = await _propriedadeRepository.GetByIdAsync(id);
        if (propriedade == null) return false;

        await _propriedadeRepository.DeleteAsync(id);
        return true;
    }

    private static PropriedadeResponse MapToResponse(Propriedade p) => new(
        p.Id,
        p.ProdutorId,
        p.Nome,
        p.Endereco,
        p.Cidade,
        p.Estado,
        p.CEP,
        p.Latitude,
        p.Longitude,
        p.AreaTotalHa,
        p.Ativo,
        p.CreatedAt,
        p.UpdatedAt
    );
}
