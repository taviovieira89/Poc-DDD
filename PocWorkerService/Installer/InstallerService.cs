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
       var kafkaConfig = configuration.GetSection("Kafka").Get<IntegrationEvent>();

        services.AddSingleton(kafkaConfig!); // Adiciona a configuração como Singleton

        services.AddScoped<ClienteEnvelope>();
      return services;
    }

}