using Microsoft.EntityFrameworkCore;
public interface IRepositories<T> : IRepository<T, PocContext>, IUnitOfWork where T : class
{
}

public interface IRepositorioMongo<T> : IMongoDbRepository<T> where T : class
{

}