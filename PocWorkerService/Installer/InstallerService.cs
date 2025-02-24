using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Design;
using PocWorkerService.Consumer;
using Microsoft.Extensions.Options;
using PocDomain.Aggregate.Cliente;
public static class InstallerService
{
  public static IServiceCollection AddServices(
                this IServiceCollection services,
                IConfiguration configuration)
  {

    var connectionString = configuration.GetConnectionString("DefaultConnection");

    services.AddDbContext<PocContext>(options => options.UseSqlServer(connectionString));

    var kafkaConfig = configuration.GetSection("Kafka").Get<IntegrationEvent>();

    services.AddSingleton(kafkaConfig!); // Adiciona a configuração como Singleton
                                         // Registra consumidores específicos

    var configiMongoDb = configuration.GetSection("MongoDbSettings");
    services.Configure<MongoDbSettings>(configiMongoDb!);
    services.AddScoped<PocContextMongo>(); 
    services.AddScoped(typeof(IRepositorioMongo<>), typeof(RepositorioMongo<>));
    services.AddScoped<IClienteMongoRepository, ClienteMongoRepository>();
    services.AddScoped(typeof(IMessageMapper<>), typeof(MessageMapper<>));
    services.AddScoped<IKafkaConsumer<IntegrationEvent>, ClienteConsumer>();
    services.AddScoped<ClienteEnvelope>();
    services.AddTransient<ResultConsumer>();
    services.AddScoped<IObterClienteUseCase, ObterClienteUseCase>();
    services.AddScoped<IClienteRepository, ClienteRepository>();
    services.AddScoped(typeof(IRequestHandler<KafkaMessageReceived<IntegrationEvent>>), typeof(KafkaMessageHandler<IntegrationEvent>));
    services.AddScoped<IUnitOfWork>(sp =>
{
  var pocContext = sp.GetRequiredService<PocContext>();
  var mediator = sp.GetRequiredService<IMediator>();
  //var mongoDbContext = sp.GetRequiredService<MongoDbContext>();

  return new UnitOfWork<PocContext>(pocContext, mediator, null!);
});

    var myhandlers = AppDomain.CurrentDomain.Load("PocInfra");
    services.AddMediatR(cfg =>
    {
      cfg.RegisterServicesFromAssemblies(myhandlers);
    });

    services.AddValidatorsFromAssembly(Assembly.Load("PocApplication"));

    return services;
  }

}