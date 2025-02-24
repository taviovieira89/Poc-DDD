
using Microsoft.Extensions.Logging;
using PocDomain.Aggregate.Cliente;

public class ObterClienteUseCase : IObterClienteUseCase
{
    private readonly ILogger<ObterClienteUseCase> _logger;
    private readonly IClienteMongoRepository _clienteRepository;

    public ObterClienteUseCase(ILogger<ObterClienteUseCase> logger, IClienteMongoRepository clienteRepository)
    {
        _logger = logger;
        _clienteRepository = clienteRepository;
    }

    public async Task Execute(ObterClienteDto dto)
    {
        _logger.LogInformation("Executando o caso de uso ObterClienteUseCase");
        //_logger.LogInformation($"Param Dto: Nome{dto.Nome},BirthDate({dto.Nascimento}.");
        var clienteResult = Cliente.Create(new Name(dto.Nome), new BirthDate(dto.Nascimento));

        if (!clienteResult.IsSuccess)
        {
            _logger.LogWarning($"Falha ao criar cliente: {clienteResult.Error}");
            return;
        }

        Cliente cliente = clienteResult.Value;
        _clienteRepository.Add(cliente);
        _logger.LogInformation("Executado com Sucesso o caso de uso ObterClienteUseCase!!!");

    }
}