using ProdutorRuralCadastro.Application.DTOs.Talhao;

namespace ProdutorRuralCadastro.Application.DTOs.Propriedade;

public record PropriedadeResponse(
    Guid Id,
    Guid ProdutorId,
    string Nome,
    string? Endereco,
    string? Cidade,
    string? Estado,
    string? CEP,
    decimal? Latitude,
    decimal? Longitude,
    decimal? AreaTotalHa,
    bool Ativo,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record PropriedadeComTalhoesResponse(
    Guid Id,
    Guid ProdutorId,
    string Nome,
    string? Endereco,
    string? Cidade,
    string? Estado,
    string? CEP,
    decimal? Latitude,
    decimal? Longitude,
    decimal? AreaTotalHa,
    bool Ativo,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IEnumerable<TalhaoResumoResponse> Talhoes
);

public record PropriedadeResumoResponse(
    Guid Id,
    string Nome,
    string? Cidade,
    string? Estado,
    decimal? AreaTotalHa
);
