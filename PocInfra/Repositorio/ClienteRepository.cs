using PocDomain.Aggregate.Cliente;
using MediatR;
public class ClienteRepository : Repositorio<Cliente>, IClienteRepository
{  
    public ClienteRepository(PocContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
}