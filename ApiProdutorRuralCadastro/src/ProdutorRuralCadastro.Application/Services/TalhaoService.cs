using ProdutorRuralCadastro.Application.DTOs.Cultura;
using ProdutorRuralCadastro.Application.DTOs.Propriedade;
using ProdutorRuralCadastro.Application.DTOs.Talhao;
using ProdutorRuralCadastro.Application.Services.Interfaces;
using ProdutorRuralCadastro.Domain.Entities;
using ProdutorRuralCadastro.Domain.Interfaces;

namespace ProdutorRuralCadastro.Application.Services;

public class TalhaoService : ITalhaoService
{
    private readonly ITalhaoRepository _talhaoRepository;
    private readonly IPropriedadeRepository _propriedadeRepository;
    private readonly ICulturaRepository _culturaRepository;

    public TalhaoService(
        ITalhaoRepository talhaoRepository,
        IPropriedadeRepository propriedadeRepository,
        ICulturaRepository culturaRepository)
    {
        _talhaoRepository = talhaoRepository;
        _propriedadeRepository = propriedadeRepository;
        _culturaRepository = culturaRepository;
    }

    public async Task<IEnumerable<TalhaoResponse>> GetAllAsync()
    {
        var talhoes = await _talhaoRepository.GetAllAtivosAsync();
        return talhoes.Select(MapToResponse);
    }

    public async Task<IEnumerable<TalhaoResponse>> GetByPropriedadeIdAsync(Guid propriedadeId)
    {
        var talhoes = await _talhaoRepository.GetByPropriedadeIdAsync(propriedadeId);
        return talhoes.Select(MapToResponse);
    }

    public async Task<IEnumerable<TalhaoComDetalhesResponse>> GetByProdutorIdAsync(Guid produtorId)
    {
        var talhoes = await _talhaoRepository.GetByProdutorIdAsync(produtorId);
        return talhoes.Select(MapToDetailedResponse);
    }

    public async Task<TalhaoResponse?> GetByIdAsync(Guid id)
    {
        var talhao = await _talhaoRepository.GetByIdAsync(id);
        return talhao == null ? null : MapToResponse(talhao);
    }

    public async Task<TalhaoComDetalhesResponse?> GetByIdWithDetailsAsync(Guid id)
    {
        var talhao = await _talhaoRepository.GetByIdWithDetailsAsync(id);
        return talhao == null ? null : MapToDetailedResponse(talhao);
    }

    public async Task<TalhaoResponse> CreateAsync(TalhaoCreateRequest request)
    {
        // Validar se propriedade existe
        var propriedade = await _propriedadeRepository.GetByIdAsync(request.PropriedadeId);
        if (propriedade == null)
            throw new ArgumentException("Propriedade não encontrada");

        // Validar se cultura existe
        var cultura = await _culturaRepository.GetByIdAsync(request.CulturaId);
        if (cultura == null)
            throw new ArgumentException("Cultura não encontrada");

        var talhao = new Talhao
        {
            PropriedadeId = request.PropriedadeId,
            CulturaId = request.CulturaId,
            Nome = request.Nome,
            Codigo = request.Codigo,
            AreaHa = request.AreaHa,
            Status = 0, // Normal
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            DataPlantio = request.DataPlantio,
            PrevisaoColheita = request.PrevisaoColheita,
            Ativo = true
        };

        var created = await _talhaoRepository.AddAsync(talhao);
        return MapToResponse(created);
    }

    public async Task<TalhaoResponse?> UpdateAsync(Guid id, TalhaoUpdateRequest request)
    {
        var talhao = await _talhaoRepository.GetByIdAsync(id);
        if (talhao == null) return null;

        // Validar se cultura existe
        var cultura = await _culturaRepository.GetByIdAsync(request.CulturaId);
        if (cultura == null)
            throw new ArgumentException("Cultura não encontrada");

        talhao.CulturaId = request.CulturaId;
        talhao.Nome = request.Nome;
        talhao.Codigo = request.Codigo;
        talhao.AreaHa = request.AreaHa;
        talhao.Latitude = request.Latitude;
        talhao.Longitude = request.Longitude;
        talhao.DataPlantio = request.DataPlantio;
        talhao.PrevisaoColheita = request.PrevisaoColheita;
        talhao.Ativo = request.Ativo;

        await _talhaoRepository.UpdateAsync(talhao);
        return MapToResponse(talhao);
    }

    public async Task<bool> UpdateStatusAsync(Guid id, TalhaoUpdateStatusRequest request)
    {
        var talhao = await _talhaoRepository.GetByIdAsync(id);
        if (talhao == null) return false;

        await _talhaoRepository.UpdateStatusAsync(id, request.Status, request.StatusDescricao);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var talhao = await _talhaoRepository.GetByIdAsync(id);
        if (talhao == null) return false;

        await _talhaoRepository.DeleteAsync(id);
        return true;
    }

    private static TalhaoResponse MapToResponse(Talhao t) => new(
        t.Id,
        t.PropriedadeId,
        t.CulturaId,
        t.Nome,
        t.Codigo,
        t.AreaHa,
        t.Status,
        t.StatusDescricao ?? t.GetStatusDescricao(),
        t.Latitude,
        t.Longitude,
        t.DataPlantio,
        t.PrevisaoColheita,
        t.Ativo,
        t.CreatedAt,
        t.UpdatedAt
    );

    private static TalhaoComDetalhesResponse MapToDetailedResponse(Talhao t) => new(
        t.Id,
        t.PropriedadeId,
        t.CulturaId,
        t.Nome,
        t.Codigo,
        t.AreaHa,
        t.Status,
        t.StatusDescricao ?? t.GetStatusDescricao(),
        t.Latitude,
        t.Longitude,
        t.DataPlantio,
        t.PrevisaoColheita,
        t.Ativo,
        t.CreatedAt,
        t.UpdatedAt,
        t.Propriedade != null ? new PropriedadeResumoResponse(
            t.Propriedade.Id,
            t.Propriedade.Nome,
            t.Propriedade.Cidade,
            t.Propriedade.Estado,
            t.Propriedade.AreaTotalHa
        ) : null,
        t.Cultura != null ? new CulturaResumoResponse(
            t.Cultura.Id,
            t.Cultura.Nome
        ) : null
    );
}
