using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PocDomain.Aggregate.Cliente;

public class FakeClienteRepository : IClienteRepository
{
    private readonly List<Cliente> _clientes = new List<Cliente>();

    public void Add(Cliente cliente)
    {
        _clientes.Add(cliente);
    }

    public async Task SaveChangesAsync()
    {
        await Task.CompletedTask; // Simula um commit no banco
    }

    public Task<Cliente?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_clientes.FirstOrDefault(c => c.IdCliente == id));
    }

    public int Count() => _clientes.Count;

    public IEnumerable<Cliente> GetAll()
    {
        return Task.FromResult(_clientes.AsEnumerable()).Result;
    }

    public async Task<IEnumerable<Cliente>> GetAllAsync()
    {
        return await Task.FromResult(_clientes.AsEnumerable());
    }

    public IEnumerable<Cliente> Find(Expression<Func<Cliente, bool>> predicate)
    {
        return Task.FromResult(_clientes.AsEnumerable().Where(predicate.Compile())).Result;
    }

    public async Task<IEnumerable<Cliente>> FindAsync(Expression<Func<Cliente, bool>> predicate)
    {
       return await Task.FromResult(_clientes.AsEnumerable().Where(predicate.Compile()));
    }

    public Cliente GetById(object id)
    {
        return _clientes.FirstOrDefault(c => c.IdCliente == Guid.Parse(id.ToString()));
    }

    public Task<Cliente> GetByIdAsync(object id)
    {
        return Task.FromResult(_clientes.FirstOrDefault(c => c.IdCliente == Guid.Parse(id.ToString())));
    }

    public void SaveChanges()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
