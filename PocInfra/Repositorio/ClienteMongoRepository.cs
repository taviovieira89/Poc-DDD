using MongoDB.Driver;
using PocDomain.Aggregate.Cliente;

public class ClienteMongoRepository : RepositorioMongo<Cliente>, IClienteMongoRepository
{
    public ClienteMongoRepository(PocContextMongo context) : base(context, "Clientes")
    {
    }
}
