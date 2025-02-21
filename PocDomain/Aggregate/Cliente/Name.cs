public record Name
{
    public string Value { get; }

    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("O nome não pode estar vazio ou conter apenas espaços em branco.", nameof(value));
        }

        if (value.Count() < 2)
        {
            throw new ArgumentException("O nome não pode ser muito curto.", nameof(value));
        }

        if (value.Count() > 100)
        {
            throw new ArgumentException("O nome não pode ser muito longo.", nameof(value));
        }


        Value = value;
    }
}
