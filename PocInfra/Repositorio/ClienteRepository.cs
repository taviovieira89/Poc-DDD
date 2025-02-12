using PocDomain.Aggregate.Cliente;
public class ClienteRepository : Repositorio<Cliente>, IClienteRepository
{  
    public ClienteRepository(PocContext context) : base(context)
    {
    }
}