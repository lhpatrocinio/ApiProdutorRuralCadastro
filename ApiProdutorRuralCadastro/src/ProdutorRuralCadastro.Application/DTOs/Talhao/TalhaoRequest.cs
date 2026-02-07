using System.ComponentModel.DataAnnotations;

namespace ProdutorRuralCadastro.Application.DTOs.Talhao;

public record TalhaoCreateRequest(
    [Required(ErrorMessage = "PropriedadeId é obrigatório")]
    Guid PropriedadeId,
    
    [Required(ErrorMessage = "CulturaId é obrigatório")]
    Guid CulturaId,
    
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
    string Nome,
    
    [StringLength(20)]
    string? Codigo,
    
    [Range(0.01, 99999.99, ErrorMessage = "Área deve ser maior que 0")]
    decimal? AreaHa,
    
    decimal? Latitude,
    decimal? Longitude,
    DateTime? DataPlantio,
    DateTime? PrevisaoColheita
);

public record TalhaoUpdateRequest(
    [Required(ErrorMessage = "CulturaId é obrigatório")]
    Guid CulturaId,
    
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
    string Nome,
    
    [StringLength(20)]
    string? Codigo,
    
    [Range(0.01, 99999.99, ErrorMessage = "Área deve ser maior que 0")]
    decimal? AreaHa,
    
    decimal? Latitude,
    decimal? Longitude,
    DateTime? DataPlantio,
    DateTime? PrevisaoColheita,
    bool Ativo
);

public record TalhaoUpdateStatusRequest(
    [Required]
    [Range(0, 2, ErrorMessage = "Status deve ser 0 (Normal), 1 (Alerta) ou 2 (Crítico)")]
    int Status,
    
    string? StatusDescricao
);

/// <summary>
/// Request para criar talhão a partir do endpoint de propriedade
/// (PropriedadeId vem da rota)
/// </summary>
public record TalhaoCreateForPropriedadeRequest(
    [Required(ErrorMessage = "CulturaId é obrigatório")]
    Guid CulturaId,
    
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
    string Nome,
    
    [StringLength(20)]
    string? Codigo,
    
    [Range(0.01, 99999.99, ErrorMessage = "Área deve ser maior que 0")]
    decimal? AreaHa,
    
    decimal? Latitude,
    decimal? Longitude,
    DateTime? DataPlantio,
    DateTime? PrevisaoColheita
);
