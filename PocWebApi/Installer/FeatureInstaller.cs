using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Design;

public static class FeatureInstaller
{


  public static IServiceCollection AddFeature(
                    this IServiceCollection services,
                    IConfiguration configuration)
  {

    var connectionString = configuration.GetConnectionString("DefaultConnection");

    services.AddDbContext<PocContext>(options => options.UseSqlServer(connectionString));
  
    // Lendo configurações do Kafka
    var kafkaConfig = configuration.GetSection("Kafka").Get<IntegrationEvent>();

    services.AddSingleton(kafkaConfig!); // Adiciona a configuração como Singleton

    services.AddScoped<ClienteEnvelope>();
    services.AddScoped<IValidator<CreateClienteDto>, CreateClienteDtoValidator>();

    // Registrar IDbConnection como uma instância única
    services.AddSingleton<IDbConnection>(provider =>
    {
      var connection = new SqlConnection(connectionString); // Usando SqlConnection para SQL Server
      connection.Open();
      return connection;
    });

    services.AddScoped<IUnitOfWork>(sp =>
{
  var pocContext = sp.GetRequiredService<PocContext>();
  var mediator = sp.GetRequiredService<IMediator>();
  //var mongoDbContext = sp.GetRequiredService<MongoDbContext>();

  return new UnitOfWork<PocContext>(pocContext, mediator, null!);
});


    services.AddScoped<ICreateClienteUseCase, CreateClienteUseCase>();
    services.AddScoped<IClienteRepository, ClienteRepository>();
    services.AddScoped(typeof(IRepositories<>), typeof(Repositorio<>));
    services.AddTransient(typeof(ResultProducer<>), typeof(ResultProducer<>));

    var myhandlers = AppDomain.CurrentDomain.Load("PocApplication");
    services.AddMediatR(cfg =>
    {
      cfg.RegisterServicesFromAssemblies(myhandlers);
    });

    services.AddValidatorsFromAssembly(Assembly.Load("PocApplication"));

    return services;
  }
}
