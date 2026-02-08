namespace ProdutorRuralCadastro.Infrastructure.Messaging.Events;

/// <summary>
/// Evento recebido quando um alerta é criado pelo serviço de Monitoramento
/// Este evento atualiza o status do talhão no serviço de Cadastro
/// </summary>
public record AlertCreatedEvent
{
    public Guid AlertaId { get; init; }
    public Guid ProdutorId { get; init; }
    public Guid TalhaoId { get; init; }
    public string TipoAlerta { get; init; } = string.Empty;  // Ex: "Umidade", "Temperatura", etc
    public string Severidade { get; init; } = string.Empty;  // Ex: "Baixa", "Media", "Alta"
    public string Titulo { get; init; } = string.Empty;
    public string? Mensagem { get; init; }
    public decimal? ValorLeitura { get; init; }
    public DateTime CreatedAt { get; init; }
}

/// <summary>
/// Mapeamento de severidade para status do talhão
/// </summary>
public static class AlertaSeveridadeMapper
{
    /// <summary>
    /// Converte severidade do alerta (string) para status do talhão (int)
    /// </summary>
    public static int ToTalhaoStatus(string severidade) => severidade?.ToLower() switch
    {
        "baixa" => 0, // Baixa -> Normal
        "media" or "média" => 1, // Média -> Alerta
        "alta" => 2, // Alta -> Crítico
        _ => 0
    };

    public static string ToStatusDescricao(string tipoAlerta, string severidade)
    {
        var tipo = tipoAlerta?.ToLower() ?? "";
        var sev = severidade?.ToLower() ?? "";
        
        return (tipo, sev) switch
        {
            ("umidade" or "umidade_solo", "media" or "média") => "Alerta - Risco de Seca",
            ("umidade" or "umidade_solo", "alta") => "Crítico - Seca Detectada",
            ("temperatura", "media" or "média") => "Alerta - Temperatura Anormal",
            ("temperatura", "alta") => "Crítico - Stress Térmico",
            ("precipitacao", "media" or "média") => "Alerta - Precipitação Anormal",
            ("precipitacao", "alta") => "Crítico - Risco de Inundação",
            _ => "Normal"
        };
    }
}
