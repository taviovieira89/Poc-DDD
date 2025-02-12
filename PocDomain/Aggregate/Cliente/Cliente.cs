namespace PocDomain.Aggregate.Cliente;

public class Cliente : AggregateRoot
{
     public Guid IdCliente { get; private set; } 
     
     public string Nome { get; private set; }

     private Cliente(){}

     public static Cliente Create(string Name){
      
        Cliente cliente = new Cliente(){
        IdCliente = Guid.NewGuid(),
        Nome  = Name
       };

       cliente.AddDomainEvent(CreateClienteDomain.Map(cliente));
       return cliente;
     }

}
