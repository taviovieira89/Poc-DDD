using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Reflection;
using Microsoft.Data.SqlClient;

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
        var kafkaConfig = builder.Configuration.GetSection("Kafka").Get<IntegrationEvent>();

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