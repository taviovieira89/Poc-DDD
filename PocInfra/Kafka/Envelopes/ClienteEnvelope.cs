public class ClienteEnvelope : IntegrationEvent
{
    public string BootstrapServers { get; set; } = default!;
    public string GroupId { get; set; } = default!;
    public string Topic { get; set; } = "PocDDD-Cliente";

    public ClienteEnvelope(string key, EventData value) : base(key, value)
    {
    }
    
    // Construtor sem parâmetros
    public ClienteEnvelope() : base("DefaultKey", new EventData()) // Valor default para 'key'
    {
    }

}