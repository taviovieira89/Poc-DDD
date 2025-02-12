using PocDomain.Aggregate.Cliente;

public class CreateClienteDomain : DomainEvent // Herda de DomainEvent (classe base)
{
    public Guid IdCliente { get; init; } // Usando init para imutabilidade
    public string Nome { get; init; } // Usando init para imutabilidade
    public DateTime Nascimento { get; init; } // Usando init para imutabilidade

    // Construtor para inicializar a classe base e as propriedades
    public CreateClienteDomain(string entityName, 
                                Guid idCliente, 
                                string nome, 
                                DateTime nascimento)
        : base(entityName) // Chama o construtor da classe base
    {
        IdCliente = idCliente;
        Nome = nome;
        Nascimento = nascimento;
    }

    // Método para mapear
    public static CreateClienteDomain Map(Cliente cliente)
    {
        return new CreateClienteDomain(
            "ClienteDomain", 
            cliente.IdCliente, 
            cliente.Nome.Value, 
            cliente.Nascimento.Value);
    }
}