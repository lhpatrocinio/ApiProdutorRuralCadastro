using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProdutorRuralCadastro.Domain.Interfaces;
using ProdutorRuralCadastro.Infrastructure.Messaging.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ProdutorRuralCadastro.Infrastructure.Messaging.Consumers;

/// <summary>
/// Consumer BackgroundService que escuta eventos de alerta criado
/// e atualiza o status dos talhões correspondentes
/// </summary>
public class AlertCreatedConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly RabbitMqSetup _rabbitMqSetup;
    private readonly ILogger<AlertCreatedConsumer> _logger;
    private IChannel? _channel;

    private const int MaxRetryAttempts = 3;

    public AlertCreatedConsumer(
        IServiceProvider serviceProvider,
        RabbitMqSetup rabbitMqSetup,
        ILogger<AlertCreatedConsumer> logger)
    {
        _serviceProvider = serviceProvider;
        _rabbitMqSetup = rabbitMqSetup;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("AlertCreatedConsumer iniciando...");

        try
        {
            _channel = await _rabbitMqSetup.GetChannelAsync();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                await ProcessMessageAsync(ea, stoppingToken);
            };

            await _channel.BasicConsumeAsync(
                queue: RabbitMqSetup.AlertCreatedQueue,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);

            _logger.LogInformation("AlertCreatedConsumer iniciado. Aguardando mensagens na queue: {Queue}",
                RabbitMqSetup.AlertCreatedQueue);

            // Mantém o consumer rodando
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("AlertCreatedConsumer sendo finalizado...");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no AlertCreatedConsumer");
        }
    }

    private async Task ProcessMessageAsync(BasicDeliverEventArgs ea, CancellationToken cancellationToken)
    {
        var messageBody = Encoding.UTF8.GetString(ea.Body.ToArray());
        var retryCount = GetRetryCount(ea.BasicProperties);

        try
        {
            _logger.LogInformation("Mensagem recebida: {Message}", messageBody);

            var alertEvent = JsonSerializer.Deserialize<AlertCreatedEvent>(messageBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (alertEvent == null)
            {
                _logger.LogWarning("Mensagem inválida recebida, descartando");
                await _channel!.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);
                return;
            }

            await ProcessAlertAsync(alertEvent, cancellationToken);

            // Acknowledge a mensagem processada com sucesso
            await _channel!.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);
            _logger.LogInformation("Alerta processado com sucesso. TalhaoId: {TalhaoId}", alertEvent.TalhaoId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar mensagem. Tentativa {RetryCount}/{MaxRetry}",
                retryCount + 1, MaxRetryAttempts);

            if (retryCount < MaxRetryAttempts - 1)
            {
                // Rejeita e recoloca na fila para retry
                await _channel!.BasicNackAsync(ea.DeliveryTag, false, true, cancellationToken);
            }
            else
            {
                // Após máximo de tentativas, envia para DLQ
                await _channel!.BasicNackAsync(ea.DeliveryTag, false, false, cancellationToken);
                _logger.LogError("Mensagem enviada para DLQ após {MaxRetry} tentativas", MaxRetryAttempts);
            }
        }
    }

    private async Task ProcessAlertAsync(AlertCreatedEvent alertEvent, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var talhaoRepository = scope.ServiceProvider.GetRequiredService<ITalhaoRepository>();

        var talhao = await talhaoRepository.GetByIdAsync(alertEvent.TalhaoId);
        if (talhao == null)
        {
            _logger.LogWarning("Talhão não encontrado: {TalhaoId}", alertEvent.TalhaoId);
            return;
        }

        // Atualiza status baseado na severidade do alerta
        var novoStatus = AlertaSeveridadeMapper.ToTalhaoStatus(alertEvent.Severidade);
        var novaDescricao = AlertaSeveridadeMapper.ToStatusDescricao(alertEvent.TipoAlerta, alertEvent.Severidade);

        talhao.Status = novoStatus;
        talhao.StatusDescricao = novaDescricao;
        talhao.UpdatedAt = DateTime.UtcNow;

        await talhaoRepository.UpdateAsync(talhao);

        _logger.LogInformation(
            "Status do talhão {TalhaoId} atualizado para {Status} ({Descricao})",
            alertEvent.TalhaoId, novoStatus, novaDescricao);
    }

    private static int GetRetryCount(IReadOnlyBasicProperties properties)
    {
        if (properties.Headers != null &&
            properties.Headers.TryGetValue("x-retry-count", out var retryCountObj) &&
            retryCountObj is int retryCount)
        {
            return retryCount;
        }
        return 0;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("AlertCreatedConsumer parando...");
        await base.StopAsync(cancellationToken);
    }
}
