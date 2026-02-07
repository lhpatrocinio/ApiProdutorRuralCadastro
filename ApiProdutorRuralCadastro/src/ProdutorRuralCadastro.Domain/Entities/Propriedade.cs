namespace ProdutorRuralCadastro.Domain.Entities;

/// <summary>
/// Entidade que representa uma propriedade rural
/// </summary>
public class Propriedade : BaseEntity
{
    public Guid ProdutorId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Endereco { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public string? CEP { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public decimal? AreaTotalHa { get; set; }
    public bool Ativo { get; set; } = true;

    // Navigation
    public virtual ICollection<Talhao> Talhoes { get; set; } = new List<Talhao>();
}
