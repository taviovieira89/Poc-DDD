using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuração de serviços
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Minha API", Version = "v1" });
});

builder.Services.AddScoped<ICreateClienteUseCase, CreateClienteUseCase>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ContextDb>(options => options.UseSqlServer(connectionString));

// Lendo configurações do Kafka
var kafkaConfig = builder.Configuration.GetSection("Kafka").Get<IntegrationEvent>();

// Registrando o producer como um Singleton
builder.Services.AddSingleton(typeof(ResultProducer<>), sp => new ResultProducer<object>(kafkaConfig));

builder.Services.AddAuthorization();
builder.Services.AddLogging();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Registrar controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configuração do pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
