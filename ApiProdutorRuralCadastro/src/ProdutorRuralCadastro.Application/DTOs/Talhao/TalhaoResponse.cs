using ProdutorRuralCadastro.Application.DTOs.Cultura;
using ProdutorRuralCadastro.Application.DTOs.Propriedade;

namespace ProdutorRuralCadastro.Application.DTOs.Talhao;

public record TalhaoResponse(
    Guid Id,
    Guid PropriedadeId,
    Guid CulturaId,
    string Nome,
    string? Codigo,
    decimal? AreaHa,
    int Status,
    string StatusDescricao,
    decimal? Latitude,
    decimal? Longitude,
    DateTime? DataPlantio,
    DateTime? PrevisaoColheita,
    bool Ativo,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record TalhaoComDetalhesResponse(
    Guid Id,
    Guid PropriedadeId,
    Guid CulturaId,
    string Nome,
    string? Codigo,
    decimal? AreaHa,
    int Status,
    string StatusDescricao,
    decimal? Latitude,
    decimal? Longitude,
    DateTime? DataPlantio,
    DateTime? PrevisaoColheita,
    bool Ativo,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    PropriedadeResumoResponse? Propriedade,
    CulturaResumoResponse? Cultura
);

public record TalhaoResumoResponse(
    Guid Id,
    string Nome,
    string? Codigo,
    decimal? AreaHa,
    int Status,
    string StatusDescricao,
    string? CulturaNome
);

/// <summary>
/// Response com detalhes expandidos para AutoMapper
/// </summary>
public record TalhaoDetailResponse(
    Guid Id,
    Guid PropriedadeId,
    Guid CulturaId,
    string Nome,
    string? Codigo,
    decimal? AreaHa,
    int Status,
    string StatusDescricao,
    decimal? Latitude,
    decimal? Longitude,
    DateTime? DataPlantio,
    DateTime? PrevisaoColheita,
    bool Ativo,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    string? CulturaNome,
    string? PropriedadeNome
);
