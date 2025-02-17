using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Design;

namespace PocCrossCutting.PocDependencies;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
                  this IServiceCollection services,
                  IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("DefaultConnection");                      

        services.AddDbContext<PocContext>(options => options.UseSqlServer(connectionString));

        // Lendo configurações do Kafka
        var kafkaConfig = configuration.GetSection("Kafka").Get<IntegrationEvent>();

       // Registrando o producer como um Singleton
       services.AddSingleton(typeof(ResultProducer<>), sp => new ResultProducer<object>(kafkaConfig!));

        // Registrar IDbConnection como uma instância única
      services.AddSingleton<IDbConnection>(provider =>
      {
        var connection = new SqlConnection(connectionString); // Usando SqlConnection para SQL Server
        connection.Open();
        return connection;
      });

        services.AddScoped<ICreateClienteUseCase, CreateClienteUseCase>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped(typeof(IRepositories<>), typeof(Repository<>));

        var myhandlers = AppDomain.CurrentDomain.Load("PocApplication");
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(myhandlers);
        });

        services.AddValidatorsFromAssembly(Assembly.Load("PocApplication"));

        return services;
    }
}

// public class PocContextFactory : IDesignTimeDbContextFactory<PocContext>
// {
//     public PocContext CreateDbContext(string[] args)
//     {
//         IConfigurationRoot configuration = new ConfigurationBuilder()
//             .SetBasePath(Directory.GetCurrentDirectory()) // Define a pasta do projeto
//             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//             .Build();

//         var connectionString = configuration.GetConnectionString("DefaultConnection");

//         var optionsBuilder = new DbContextOptionsBuilder<PocContext>();
//         optionsBuilder.UseSqlServer(connectionString); // Ajuste para seu banco (SQL Server, MySQL, etc.)

//         return new PocContext(optionsBuilder.Options);
//     }
// }