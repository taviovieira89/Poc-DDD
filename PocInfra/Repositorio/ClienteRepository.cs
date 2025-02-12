using PocDomain.Aggregate.Cliente;
public class ClienteRepository : Repositorio<Cliente>, IClienteRepository
{  
    public ClienteRepository(ContextDb context) : base(context)
    {
    }
}