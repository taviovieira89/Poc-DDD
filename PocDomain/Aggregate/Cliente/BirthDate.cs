public record BirthDate
{
    public DateTime Value { get; }

    public BirthDate(DateTime value)
    {
        if (value == default)
        {
            throw new ArgumentException("A data de nascimento não pode ser inválida.", nameof(value));
        }

        if (value > DateTime.Now)
        {
            throw new ArgumentException("A data de nascimento não pode ser maior que a data atual.", nameof(value));
        }

        Value = value;
    }
}