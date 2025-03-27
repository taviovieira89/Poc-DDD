using Microsoft.Extensions.Logging;
using PocDomain.Aggregate.Cliente;
public class CreateClienteUseCase : ICreateClienteUseCase
{
    private readonly IClienteRepository _repository;
    private readonly ILogger<CreateClienteUseCase> _logger;
    public CreateClienteUseCase(
        IClienteRepository repository,
        ILogger<CreateClienteUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }


    public async Task<Result<Guid>> Execute(CreateClienteDto Value)
    {

        var clienteResult = Cliente.Create(
            new Name(Value.Nome),
            new BirthDate(Value.Nascimento)
        );

        if (!clienteResult.IsSuccess)
        {
            return Result<Guid>.Failure(clienteResult.Error);
        }

        Cliente cliente = clienteResult.Value;
        _repository.Add(cliente);
        await _repository.SaveChangesAsync();

        return Result<Guid>.Success(cliente.IdCliente);
    }

}