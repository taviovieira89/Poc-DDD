using Confluent.Kafka;

public class ClienteEnvelope : IntegrationEvent
{
    public new string BootstrapServers { get; set; } = "localhost:9092";
    public new string GroupId { get; set; } = "PocDDD";
    public  new string Topic { get; set; } = "PocDDD-Cliente";

    public new AutoOffsetReset AutoOffsetReset { get; set; } = AutoOffsetReset.Earliest;

    public ClienteEnvelope(string key, EventData value) : base(key, value)
    {
    }
    
    // Construtor sem parâmetros
    public ClienteEnvelope() : base("DefaultKey", new EventData()) // Valor default para 'key'
    {
    }

     public static IntegrationEvent PassValue(ClienteEnvelope envelope){
            return new IntegrationEvent
            {
                Topic = envelope.Topic,  
                BootstrapServers = envelope.BootstrapServers,
                GroupId = envelope.GroupId 
            };
     }
}