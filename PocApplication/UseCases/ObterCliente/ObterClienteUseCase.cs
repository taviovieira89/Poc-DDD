
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
        var retorno = await _clienteRepository.GetAllAsync();
        if (retorno.Any(x => x.Nome.Value == dto.Nome))
        {
            throw new ClienteException("Cliente já cadastrado");
        }

        var clienteResult = Cliente.Create(new Name(dto.Nome), new BirthDate(dto.Nascimento));

        if (!clienteResult.IsSuccess)
        {
            throw new ClienteException(clienteResult.Error);
        }

        Cliente cliente = clienteResult.Value;
        _clienteRepository.Add(cliente);
    }
}