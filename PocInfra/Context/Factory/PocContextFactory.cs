using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
public class PocContextFactory : IDesignTimeDbContextFactory<PocContext>
{
    public PocContext CreateDbContext(string[] args)
    {
        // Configuração para o arquivo appsettings.json
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Define a pasta onde o projeto está localizado
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Obtém a string de conexão
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Cria a instância do DbContextOptions
        var optionsBuilder = new DbContextOptionsBuilder<PocContext>();
        optionsBuilder.UseSqlServer(connectionString); // Ajuste para seu banco SQL Server

        return new PocContext(optionsBuilder.Options);
    }
}