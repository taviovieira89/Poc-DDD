public record BirthDate
{
    public DateTime Value { get; }

    public BirthDate(DateTime value)
    {
        if (value == default)
        {
            throw new ArgumentException("A data de nascimento não pode ser inválida.", nameof(value));
        }
        Value = value;
    }
}