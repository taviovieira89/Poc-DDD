using System.Text.Json;

public interface IMessageMapper<TDto>
{
    TDto MapToDto(EventData message);
}

public class MessageMapper<TDto> : IMessageMapper<TDto>
{
    public TDto MapToDto(EventData message)
    {
        return JsonSerializer.Deserialize<TDto>(message.Value, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}
