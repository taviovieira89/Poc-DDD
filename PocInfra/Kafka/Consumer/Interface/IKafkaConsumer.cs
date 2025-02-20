public interface IKafkaConsumer<IntegrationEvent>
{
    Task ConsumeAsync(IntegrationEvent message, CancellationToken cancellationToken);
}