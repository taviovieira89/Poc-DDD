using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Design;
public static class InstallerService
{
    public static IServiceCollection AddServices(
                  this IServiceCollection services,
                  IConfiguration configuration)
    {
        // Lendo configurações do Kafka
        var kafkaConfig = configuration.GetSection("Kafka").Get<IntegrationEvent>();
       
       if(kafkaConfig!=null){
          // Registrando o producer como um Singleton
          services.AddSingleton(typeof(ResultConsumer), sp => new ResultConsumer(kafkaConfig!));
       }
      return services;
    }

}