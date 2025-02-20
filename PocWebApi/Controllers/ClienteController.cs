using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using PocDomain.Aggregate.Cliente;

namespace PocWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ICreateClienteUseCase _createClienteUseCase;
        private readonly ILogger<ClienteController> _logger;

        public ClienteController(ICreateClienteUseCase createClienteUseCase, ILogger<ClienteController> logger)
        {
            _createClienteUseCase = createClienteUseCase;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCliente([FromBody] CreateClienteDto createClienteDto)
        {
            if (createClienteDto == null || string.IsNullOrWhiteSpace(createClienteDto.Nome))
            {
                _logger.LogWarning("Tentativa de criação de cliente com nome vazio.");
                return BadRequest("O nome do cliente é obrigatório.");
            }

            try
            {
                var result = await _createClienteUseCase.Execute(createClienteDto);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning($"Falha ao criar cliente: {result.Error}");
                    return BadRequest(result.Error);
                }

                _logger.LogInformation($"Cliente criado com sucesso: {result.Value}");
                return CreatedAtAction(nameof(CreateCliente), new { id = result.Value }, result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar cliente.");
                return StatusCode(500, "Erro inesperado ao criar cliente.");
            }
        }

        [HttpGet("HealthCheck")]
        public IActionResult HealthCheck()
        {
            _logger.LogInformation("HealthCheck endpoint chamado.");
            return Ok("Cliente API is running...");
        }
    }
}
