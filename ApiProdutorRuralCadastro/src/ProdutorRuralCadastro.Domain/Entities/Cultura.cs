namespace ProdutorRuralCadastro.Domain.Entities;

/// <summary>
/// Entidade que representa uma cultura agrícola (Soja, Milho, Café, etc.)
/// </summary>
public class Cultura : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal? UmidadeIdealMin { get; set; }
    public decimal? UmidadeIdealMax { get; set; }
    public decimal? TempIdealMin { get; set; }
    public decimal? TempIdealMax { get; set; }
    public bool Ativo { get; set; } = true;

    // Navigation
    public virtual ICollection<Talhao> Talhoes { get; set; } = new List<Talhao>();
}
