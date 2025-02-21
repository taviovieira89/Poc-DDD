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
          try
        {
            var clienteResult = Cliente.Create(
                new Name(Value.Nome),
                new BirthDate(Value.Nascimento)
            );

            if (!clienteResult.IsSuccess)
            {
                _logger.LogWarning($"Falha ao criar cliente: {clienteResult.Error}");
                return Result<Guid>.Failure(clienteResult.Error);
            }

            Cliente cliente = clienteResult.Value;
            _repository.Add(cliente);
            await _repository.SaveChangesAsync();

            return Result<Guid>.Success(cliente.IdCliente);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro inesperado ao criar cliente: {ex.Message}");
            throw new ClienteException("Ocorreu um erro inesperado ao criar o cliente.");
        }
     }

}