public record BirthDate
{
    public DateTime Value { get; }

    public BirthDate(DateTime value)
    {
        if (value == default)
        {
            throw new ArgumentException($"A data de nascimento {value.ToString("dd/MM/yyyy")} não pode ser inválida.");
        }

        if (value > DateTime.Now)
        {
            throw new ArgumentException($"A data de nascimento {value.ToString("dd/MM/yyyy")} não pode ser maior que a data atual.");
        }

        Value = value;
    }
}