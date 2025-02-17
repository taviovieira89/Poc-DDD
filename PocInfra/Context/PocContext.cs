using PocDomain.Aggregate.Cliente;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
public class PocContext : DbContext
{
    //public PocContext(){}

    public PocContext(DbContextOptions<PocContext> options) : base(options) // Aqui, passe o tipo correto para a base
    {
    }
    public DbSet<Cliente> Clientes { get; set; }

    // Outros DbSets e configurações

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ClienteConfiguration());
    }
}

