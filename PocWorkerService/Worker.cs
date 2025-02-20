using MediatR;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PocWorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope()) // Criar escopo aqui
                {
                    var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                        var mensagem = new KafkaMessageReceived<IntegrationEvent>(new IntegrationEvent());
                        await _mediator.Send(mensagem, stoppingToken);
                    }
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }

        catch (TaskCanceledException)
        {
            // Esta exceção é esperada quando o serviço é cancelado
            _logger.LogInformation("Worker has been cancelled.");
        }
        catch (Exception ex)
        {
            // Trate outras exceções de forma adequada
            _logger.LogError(ex, "An error occurred while executing the worker.");
        }
        finally
        {
            // Aqui você pode adicionar a lógica de limpeza ou fechamento
            _logger.LogInformation("Worker has stopped.");
        }
    }
}
