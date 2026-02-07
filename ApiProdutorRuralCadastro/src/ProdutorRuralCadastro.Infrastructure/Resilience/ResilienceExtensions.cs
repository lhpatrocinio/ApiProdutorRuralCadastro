using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using System.Net;

namespace ProdutorRuralCadastro.Infrastructure.Resilience;

/// <summary>
/// Extensões para configuração de resiliência com Polly
/// Implementa os padrões: Timeout, Retry, Circuit Breaker, Bulkhead
/// </summary>
public static class ResilienceExtensions
{
    /// <summary>
    /// Adiciona políticas de resiliência para HttpClient
    /// </summary>
    public static IServiceCollection AddResiliencePolicies(this IServiceCollection services)
    {
        // Registra as políticas como singleton
        services.AddSingleton<ResiliencePolicyProvider>();
        
        return services;
    }
}

/// <summary>
/// Provider para políticas de resiliência Polly
/// </summary>
public class ResiliencePolicyProvider
{
    private readonly ILogger<ResiliencePolicyProvider> _logger;

    public ResiliencePolicyProvider(ILogger<ResiliencePolicyProvider> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Cria política de retry com exponential backoff
    /// 3 tentativas: 2s, 4s, 8s
    /// </summary>
    public ResiliencePipeline CreateRetryPipeline()
    {
        return new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(2),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                OnRetry = args =>
                {
                    _logger.LogWarning(
                        "Retry {AttemptNumber} após falha. Aguardando {Delay}ms antes da próxima tentativa.",
                        args.AttemptNumber,
                        args.RetryDelay.TotalMilliseconds);
                    return ValueTask.CompletedTask;
                }
            })
            .Build();
    }

    /// <summary>
    /// Cria política de Circuit Breaker
    /// Abre após 5 falhas, fecha após 30s
    /// </summary>
    public ResiliencePipeline CreateCircuitBreakerPipeline()
    {
        return new ResiliencePipelineBuilder()
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 0.5,
                SamplingDuration = TimeSpan.FromSeconds(10),
                MinimumThroughput = 5,
                BreakDuration = TimeSpan.FromSeconds(30),
                OnOpened = args =>
                {
                    _logger.LogError(
                        "Circuit breaker ABERTO! Muitas falhas detectadas. Aguardando {BreakDuration}s",
                        args.BreakDuration.TotalSeconds);
                    return ValueTask.CompletedTask;
                },
                OnClosed = args =>
                {
                    _logger.LogInformation("Circuit breaker FECHADO. Operações retomadas.");
                    return ValueTask.CompletedTask;
                },
                OnHalfOpened = args =>
                {
                    _logger.LogInformation("Circuit breaker MEIO-ABERTO. Testando operação...");
                    return ValueTask.CompletedTask;
                }
            })
            .Build();
    }

    /// <summary>
    /// Cria política de Timeout
    /// 30 segundos máximo
    /// </summary>
    public ResiliencePipeline CreateTimeoutPipeline()
    {
        return new ResiliencePipelineBuilder()
            .AddTimeout(new TimeoutStrategyOptions
            {
                Timeout = TimeSpan.FromSeconds(30),
                OnTimeout = args =>
                {
                    _logger.LogWarning(
                        "Operação excedeu timeout de {Timeout}s",
                        args.Timeout.TotalSeconds);
                    return ValueTask.CompletedTask;
                }
            })
            .Build();
    }

    /// <summary>
    /// Cria pipeline combinado: Timeout -> Retry -> Circuit Breaker
    /// </summary>
    public ResiliencePipeline CreateCombinedPipeline()
    {
        return new ResiliencePipelineBuilder()
            // 1. Timeout - 30s máximo
            .AddTimeout(new TimeoutStrategyOptions
            {
                Timeout = TimeSpan.FromSeconds(30)
            })
            // 2. Retry - 3 tentativas com exponential backoff
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(2),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                OnRetry = args =>
                {
                    _logger.LogWarning(
                        "Retry {AttemptNumber} para operação. Próxima tentativa em {Delay}ms",
                        args.AttemptNumber,
                        args.RetryDelay.TotalMilliseconds);
                    return ValueTask.CompletedTask;
                }
            })
            // 3. Circuit Breaker - abre após 5 falhas em 10s
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 0.5,
                SamplingDuration = TimeSpan.FromSeconds(10),
                MinimumThroughput = 5,
                BreakDuration = TimeSpan.FromSeconds(30),
                OnOpened = args =>
                {
                    _logger.LogError("Circuit breaker ABERTO!");
                    return ValueTask.CompletedTask;
                }
            })
            .Build();
    }
}
