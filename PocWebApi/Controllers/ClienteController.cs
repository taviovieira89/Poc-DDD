using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using PocDomain.Aggregate.Cliente;
using FluentValidation;

namespace PocWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ICreateClienteUseCase _createClienteUseCase;
        private readonly ILogger<ClienteController> _logger;

        private readonly IValidator<CreateClienteDto> _validator;

        public ClienteController(
            ICreateClienteUseCase createClienteUseCase,
         ILogger<ClienteController> logger,
         IValidator<CreateClienteDto> validator)
        {
            _createClienteUseCase = createClienteUseCase;
            _logger = logger;
            _validator = validator;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCliente([FromBody] CreateClienteDto createClienteDto)
        {
            var validationResult = await _validator.ValidateAsync(createClienteDto);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning($"Falha no clienteDto: {String.Join(',', validationResult.Errors.Select(e => e.ErrorMessage))}");
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }
            var result = await _createClienteUseCase.Execute(createClienteDto);

            if (!result.IsSuccess)
            {
                _logger.LogWarning($"Falha ao criar cliente: {result.Error}");
                return BadRequest(result.Error);
            }

            _logger.LogInformation($"Cliente criado com sucesso: {result.Value}");
            return CreatedAtAction(nameof(CreateCliente), new { id = result.Value }, result.Value);
        }

        [HttpGet("HealthCheck")]
        public IActionResult HealthCheck()
        {
            _logger.LogInformation("HealthCheck endpoint chamado.");
            return Ok("Cliente API is running...");
        }
    }
}
