using MediatR;

public class KafkaMessageReceived<IntegrationEvent> : IRequest
{
    public IntegrationEvent Message { get; }

    public KafkaMessageReceived(IntegrationEvent message)
    {
        Message = message;
    }
}
