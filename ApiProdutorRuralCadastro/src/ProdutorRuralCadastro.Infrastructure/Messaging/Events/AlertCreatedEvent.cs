namespace ProdutorRuralCadastro.Infrastructure.Messaging.Events;

/// <summary>
/// Evento recebido quando um alerta é criado pelo serviço de Monitoramento
/// Este evento atualiza o status do talhão no serviço de Cadastro
/// </summary>
public record AlertCreatedEvent
{
    public Guid AlertaId { get; init; }
    public Guid TalhaoId { get; init; }
    public int TipoAlerta { get; init; }  // 1=Seca, 2=Temperatura, 3=Precipitacao
    public int Severidade { get; init; }  // 1=Baixa, 2=Media, 3=Alta
    public string Titulo { get; init; } = string.Empty;
    public string? Mensagem { get; init; }
    public decimal? ValorDetectado { get; init; }
    public decimal? ValorLimite { get; init; }
    public DateTime CreatedAt { get; init; }
}

/// <summary>
/// Mapeamento de severidade para status do talhão
/// </summary>
public static class AlertaSeveridadeMapper
{
    /// <summary>
    /// Converte severidade do alerta para status do talhão
    /// </summary>
    public static int ToTalhaoStatus(int severidade) => severidade switch
    {
        1 => 0, // Baixa -> Normal
        2 => 1, // Média -> Alerta
        3 => 2, // Alta -> Crítico
        _ => 0
    };

    public static string ToStatusDescricao(int tipoAlerta, int severidade) => (tipoAlerta, severidade) switch
    {
        (1, 2) => "Alerta - Risco de Seca",
        (1, 3) => "Crítico - Seca Detectada",
        (2, 2) => "Alerta - Temperatura Anormal",
        (2, 3) => "Crítico - Stress Térmico",
        (3, 2) => "Alerta - Precipitação Anormal",
        (3, 3) => "Crítico - Risco de Inundação",
        _ => "Normal"
    };
}
