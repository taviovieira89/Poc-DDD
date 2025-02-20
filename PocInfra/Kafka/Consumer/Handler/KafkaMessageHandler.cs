using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

public class KafkaMessageHandler<IntegrationEvent> : IRequestHandler<KafkaMessageReceived<IntegrationEvent>> 
{
    private readonly IKafkaConsumer<IntegrationEvent> _consumer;

    public KafkaMessageHandler(IKafkaConsumer<IntegrationEvent> consumer)
    {
        _consumer = consumer;
    }

    public async Task Handle(KafkaMessageReceived<IntegrationEvent> request, CancellationToken cancellationToken)
    {
        await _consumer.ConsumeAsync(request.Message, cancellationToken);
    }
}
