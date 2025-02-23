using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PocDomain.Aggregate.Cliente;

public class PocContextMongo : MongoDbContext
{
    // public IMongoCollection<Cliente> Clientes => GetCollection<Cliente>("Clientes");
    // public PocContextMongo(string Connectionstrings, string databaseName) : base(Connectionstrings, databaseName) { }
    // public PocContextMongo() : base(null!, null!) { }

     public IMongoCollection<Cliente> Clientes => GetCollection<Cliente>("Clientes");

    public PocContextMongo(IOptions<MongoDbSettings> mongoSettings)
        : base(mongoSettings.Value.ConnectionString, mongoSettings.Value.DatabaseName) { }
}
