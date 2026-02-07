namespace ProdutorRuralCadastro.Application.DTOs.Cultura;

public record CulturaResponse(
    Guid Id,
    string Nome,
    string? Descricao,
    decimal? UmidadeIdealMin,
    decimal? UmidadeIdealMax,
    decimal? TempIdealMin,
    decimal? TempIdealMax,
    bool Ativo
);

public record CulturaResumoResponse(
    Guid Id,
    string Nome
);
