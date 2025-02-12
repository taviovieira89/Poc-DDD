using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PocDomain.Aggregate.Cliente;

namespace PocWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ICreateClienteUseCase _createClienteUseCase;

        // Injeção de dependência do caso de uso
        public ClienteController(ICreateClienteUseCase createClienteUseCase)
        {
            _createClienteUseCase = createClienteUseCase;
        }

        // Endpoint para criar um cliente
        [HttpPost("Create")]
        public async Task<IActionResult> CreateCliente([FromBody] CreateClienteDto createClienteDto)
        {
            if (createClienteDto == null || string.IsNullOrWhiteSpace(createClienteDto.Nome))
            {
                return BadRequest("O nome do cliente é obrigatório.");
            }

            try
            {
                // Chama o caso de uso para criar o cliente
                await _createClienteUseCase.Execute(createClienteDto);

                // Retorna uma resposta 201 (Created) com a localização do novo recurso
                return CreatedAtAction(nameof(CreateCliente), new { id = createClienteDto.Nome }, createClienteDto);
            }
            catch (System.Exception ex)
            {
                // Retorna uma resposta 500 (Internal Server Error) em caso de erro
                return StatusCode(500, $"Erro ao criar o cliente: {ex.Message}");
            }
        }
    }
}