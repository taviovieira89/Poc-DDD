using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PocDomain.Aggregate.Cliente;

public class FakeClienteMongoRepository : IClienteMongoRepository
{
    private readonly List<Cliente> _clientes = new List<Cliente>();
    public void Add(Cliente entity)
    {
         _clientes.Add(entity);
    }

    public IEnumerable<Cliente> GetAll()
    {
        return Task.FromResult(_clientes.AsEnumerable()).Result;        
    }

    public async Task<IEnumerable<Cliente>> GetAllAsync()
    {
        return await Task.FromResult(_clientes.AsEnumerable());
    }

    public int Count() => _clientes.Count;
}