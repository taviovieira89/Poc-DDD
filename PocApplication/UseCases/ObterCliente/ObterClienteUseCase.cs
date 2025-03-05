
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
        try
        {
            _logger.LogInformation("Executando o caso de uso ObterClienteUseCase");

            var retorno = await _clienteRepository.GetAllAsync();
            if (retorno.Any(x => x.Nome.Value == dto.Nome))
            {
                _logger.LogWarning($"Cliente já cadastrado");
                throw new ClienteException("Cliente já cadastrado");
            }

            var clienteResult = Cliente.Create(new Name(dto.Nome), new BirthDate(dto.Nascimento));

            if (!clienteResult.IsSuccess)
            {
                _logger.LogWarning($"Falha ao criar cliente: {clienteResult.Error}");
                throw new ClienteException(clienteResult.Error);
            }

            Cliente cliente = clienteResult.Value;
            _clienteRepository.Add(cliente);
            _logger.LogInformation("Executado com Sucesso o caso de uso ObterClienteUseCase!!!");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao executar o caso de uso ObterClienteUseCase: {ex.Message}");
            throw new ClienteException($"Erro ao executar o caso de uso ObterClienteUseCase: {ex.Message}", ex);
        }
    }
}