namespace PocDomain.Aggregate.Cliente;

public class Cliente : AggregateRoot
{
     public Guid IdCliente { get; private set; }

     public Name Nome { get; private set; } = default!;

     public BirthDate Nascimento { get; private set; } = default!;

     public virtual bool MaiorIdade() => (DateTime.Now - this.Nascimento.Value).TotalDays / 365.25 >= 18;

     public static bool MaiorIdade(BirthDate Nascimento) => (DateTime.Now - Nascimento.Value).TotalDays / 365.25 >= 18;

     private Cliente() { }

     public static Result<Cliente> Create(Name nome, BirthDate nascimento)
    {
        if (!MaiorIdade(nascimento))
        {
            return Result<Cliente>.Failure("Cliente menor de idade");
        }

        Cliente cliente = new Cliente()
        {
            IdCliente = Guid.NewGuid(),
            Nome = nome,
            Nascimento = nascimento
        };

        cliente.AddDomainEvent(CreateClienteDomain.Map(cliente));
        return Result<Cliente>.Success(cliente);
    }

}
