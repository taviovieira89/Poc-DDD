using MediatR;
public class CreatedClienteDomainHandler : INotificationHandler<CreateClienteDomain> 
{
    private readonly IClienteRepository _repository;
    private readonly ResultProducer<CreateClienteDomain> _producer;
    private readonly ClienteEnvelope _envelope;
    
    public CreatedClienteDomainHandler(
        IClienteRepository repository, 
        ResultProducer<CreateClienteDomain> producer)
    {
       _repository = repository;
       _producer = producer;
    }

    public async Task Handle(CreateClienteDomain notification, CancellationToken cancellationToken)
    {
        var clienteExiste = Task.Run(() => _repository.GetAllAsync()).Result.Where(x=>x.IdCliente == notification.IdCliente).Any();


       await _producer.SendMessageAsync(notification , _envelope.Topic);
    }
}
