namespace PocDomain.Aggregate.Cliente;

public class Cliente : AggregateRoot
{
     public Guid IdCliente { get; private set; } 
     
     public Name Nome { get; private set; }

     public BirthDate Nascimento  { get; private set; }

     public virtual  bool MaiorIdade() => (DateTime.Now - this.Nascimento.Value).TotalDays / 365.25 >= 18;

     private Cliente(){}

     public static Cliente Create(Name Name, BirthDate Nascimento){
      
        Cliente cliente = new Cliente(){
        IdCliente = Guid.NewGuid(),
        Nome  = Name,
        Nascimento = Nascimento
       };

       cliente.AddDomainEvent(CreateClienteDomain.Map(cliente));
       return cliente;
     }

}
