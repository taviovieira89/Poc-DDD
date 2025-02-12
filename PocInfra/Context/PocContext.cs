using PocDomain.Aggregate.Cliente;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
public class PocContext : ContextDb
{
    public PocContext(DbContextOptions<ContextDb> options) : base(options) // Aqui, passe o tipo correto para a base
    {
    }
    public DbSet<Cliente> Clientes { get; set; }

    // Outros DbSets e configurações

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}



