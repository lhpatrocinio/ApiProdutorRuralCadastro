namespace ProdutorRuralCadastro.Domain.Entities;

/// <summary>
/// Entidade que representa um talhão (área de plantio) dentro de uma propriedade
/// </summary>
public class Talhao : BaseEntity
{
    public Guid PropriedadeId { get; set; }
    public Guid CulturaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Codigo { get; set; }
    public decimal? AreaHa { get; set; }
    public int Status { get; set; } = 0; // 0=Normal, 1=Alerta, 2=Crítico
    public string? StatusDescricao { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public DateTime? DataPlantio { get; set; }
    public DateTime? PrevisaoColheita { get; set; }
    public bool Ativo { get; set; } = true;

    // Navigation
    public virtual Propriedade? Propriedade { get; set; }
    public virtual Cultura? Cultura { get; set; }

    // Helpers
    public string GetStatusDescricao() => Status switch
    {
        0 => "Normal",
        1 => "Alerta",
        2 => "Crítico",
        _ => "Desconhecido"
    };
}
