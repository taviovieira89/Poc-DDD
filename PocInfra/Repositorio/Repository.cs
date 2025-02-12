using Microsoft.EntityFrameworkCore;
public class Repositorio<T> : IRepositories<T> where T : class
{
    protected readonly ContextDb _context; // Contexto do banco de dados

    public Repositorio(ContextDb context)
    {
        _context = context;
    }

    // Implementação dos métodos de IRepository<T>
    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    // Implementação dos métodos de IUnitOfWork
    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    // Implementação de IDisposable (herdado de IUnitOfWork)
    public void Dispose()
    {
        _context.Dispose();
    }
}