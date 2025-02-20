using MediatR;
using Microsoft.Extensions.Logging;
public class CreatedClienteDomainHandler : INotificationHandler<CreateClienteDomain>
{
    private readonly IClienteRepository _repository;
    private readonly ResultProducer<CreateClienteDomain> _producer;
    private readonly ClienteEnvelope _envelope;
    private ILogger<CreatedClienteDomainHandler> _logger;

    public CreatedClienteDomainHandler(
        IClienteRepository repository,
        ResultProducer<CreateClienteDomain> producer,
        ClienteEnvelope envelope,
        ILogger<CreatedClienteDomainHandler> logger)
    {
        _repository = repository;
        _producer = producer;
        _envelope = envelope;
        _logger = logger;
    }

    public async Task Handle(CreateClienteDomain notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando o evento de enviar a mensagem pra o kafka.");
        var clienteExiste = Task.Run(() => _repository.GetAllAsync()).Result.Where(x => x.IdCliente == notification.IdCliente).Any();

        if (!clienteExiste)
        {
            _logger.LogInformation($"Cliente {notification.IdCliente} não existe");
        }

        _logger.LogInformation($"O Topico é {_envelope.Topic}");
        await _producer.SendMessageAsync(notification, _envelope.Topic);
    }
}
