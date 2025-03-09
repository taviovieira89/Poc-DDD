
using MongoDB.Driver;

public class RepositorioMongo<T> : IRepositorioMongo<T> where T : class
{
    protected readonly PocContextMongo _context;   
    private readonly IMongoCollection<T> _collection; 
    public RepositorioMongo(PocContextMongo context, string collectionName)
    {
        _context = context;
        _collection = context.GetCollection<T>(collectionName);
    }

   public void Add(T entity)
    {
        _collection.InsertOneAsync(entity);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.Find((T _) => true).ToListAsync();
    }

    public IEnumerable<T> GetAll()
    {
        return _collection.Find((T _) => true).ToList();
    }
}