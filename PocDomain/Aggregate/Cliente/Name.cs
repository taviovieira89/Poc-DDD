public record Name
{
    public string Value { get; }

    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("O nome não pode estar vazio ou conter apenas espaços em branco.", nameof(value));
        }
        
        Value = value;
    }
}
