using System.Reflection;
using PocWorkerService;
using MediatR; // Certifique-se de adicionar essa referência

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddServices(builder.Configuration);

var host = builder.Build();

// 🔥 Verificação do MediatR 🔥
using var scope = host.Services.CreateScope();
var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
Console.WriteLine($"Mediator carregado com sucesso: {mediator != null}");


host.Run();
