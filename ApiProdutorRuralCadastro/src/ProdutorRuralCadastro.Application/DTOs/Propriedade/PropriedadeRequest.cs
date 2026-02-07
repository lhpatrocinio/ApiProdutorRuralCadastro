using System.ComponentModel.DataAnnotations;

namespace ProdutorRuralCadastro.Application.DTOs.Propriedade;

public record PropriedadeCreateRequest(
    [Required(ErrorMessage = "ProdutorId é obrigatório")]
    Guid ProdutorId,
    
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 200 caracteres")]
    string Nome,
    
    string? Endereco,
    string? Cidade,
    
    [StringLength(2, ErrorMessage = "Estado deve ter 2 caracteres")]
    string? Estado,
    
    string? CEP,
    decimal? Latitude,
    decimal? Longitude,
    
    [Range(0.01, 999999.99, ErrorMessage = "Área deve ser maior que 0")]
    decimal? AreaTotalHa
);

public record PropriedadeUpdateRequest(
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 200 caracteres")]
    string Nome,
    
    string? Endereco,
    string? Cidade,
    
    [StringLength(2, ErrorMessage = "Estado deve ter 2 caracteres")]
    string? Estado,
    
    string? CEP,
    decimal? Latitude,
    decimal? Longitude,
    
    [Range(0.01, 999999.99, ErrorMessage = "Área deve ser maior que 0")]
    decimal? AreaTotalHa,
    
    bool Ativo
);
