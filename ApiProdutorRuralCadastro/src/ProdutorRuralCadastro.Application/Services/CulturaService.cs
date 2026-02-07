using ProdutorRuralCadastro.Application.DTOs.Cultura;
using ProdutorRuralCadastro.Application.Services.Interfaces;
using ProdutorRuralCadastro.Domain.Interfaces;

namespace ProdutorRuralCadastro.Application.Services;

public class CulturaService : ICulturaService
{
    private readonly ICulturaRepository _culturaRepository;

    public CulturaService(ICulturaRepository culturaRepository)
    {
        _culturaRepository = culturaRepository;
    }

    public async Task<IEnumerable<CulturaResponse>> GetAllAsync()
    {
        var culturas = await _culturaRepository.GetAllAtivosAsync();
        return culturas.Select(c => new CulturaResponse(
            c.Id,
            c.Nome,
            c.Descricao,
            c.UmidadeIdealMin,
            c.UmidadeIdealMax,
            c.TempIdealMin,
            c.TempIdealMax,
            c.Ativo
        ));
    }

    public async Task<CulturaResponse?> GetByIdAsync(Guid id)
    {
        var cultura = await _culturaRepository.GetByIdAsync(id);
        if (cultura == null) return null;

        return new CulturaResponse(
            cultura.Id,
            cultura.Nome,
            cultura.Descricao,
            cultura.UmidadeIdealMin,
            cultura.UmidadeIdealMax,
            cultura.TempIdealMin,
            cultura.TempIdealMax,
            cultura.Ativo
        );
    }
}
