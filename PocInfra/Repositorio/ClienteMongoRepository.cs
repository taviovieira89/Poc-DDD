using MongoDB.Driver;
using PocDomain.Aggregate.Cliente;

public class ClienteMongoRepository : MongoRepository<Cliente>, IClienteMongoRepository
{
    private readonly IMongoCollection<Cliente> _collection;
    public ClienteMongoRepository(PocContextMongo context, string collectionName) : base(context, collectionName)
    {
        _collection = context.GetCollection<Cliente>(collectionName);
    }

    public void Add(Cliente entity)
    {
        _collection.InsertOne(entity);
    }

    public async Task<IEnumerable<Cliente>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public IEnumerable<Cliente> GetAll()
    {
        return _collection.Find(_ => true).ToList();
    }
}