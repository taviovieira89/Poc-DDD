using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MediatR;

public class Repositorio<T> : IRepositories<T> where T : class
{
    protected readonly PocContext _context; // Contexto do banco de dados
    private readonly IUnitOfWork _unitOfWork;
    public Repositorio(PocContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
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
        _unitOfWork.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await _unitOfWork.SaveChangesAsync();
    }

    // Implementação de IDisposable (herdado de IUnitOfWork)
    public void Dispose()
    {
        _context.Dispose();
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().Where(predicate).ToList();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    // Novo método para buscar pelo ID
    public T GetById(object id)
    {
        return _context.Set<T>().Find(id)!;
    }

    public async Task<T> GetByIdAsync(object id) => await _context.Set<T>().FindAsync(id) ?? throw new InvalidOperationException("Entity not found");
}