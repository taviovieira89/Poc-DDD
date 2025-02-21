using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using PocDomain.Aggregate.Cliente;
using Xunit;

public class CreateClienteUseCaseIntegrationTests
{
    private readonly FakeClienteRepository _repository;
    private readonly CreateClienteUseCase _useCase;
    private readonly ILogger<CreateClienteUseCase> _logger;
    public CreateClienteUseCaseIntegrationTests()
    {
        _repository = new FakeClienteRepository();
        _logger = new LoggerFactory().CreateLogger<CreateClienteUseCase>();

        _useCase = new CreateClienteUseCase(_repository, _logger);
    }

    [Fact]
    public async Task Execute_DeveCriarClienteComSucesso()
    {
        // Arrange
        var dto = new CreateClienteDto { Nome = "Maria", Nascimento = new DateTime(1985, 5, 20) };

        // Act
        var result = await _useCase.Execute(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repository.Count().Should().Be(1);
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Execute_NaoDeveSalvarClienteInvalido()
    {
        // Arrange
        string errorMessage = "O nome não pode estar vazio ou conter apenas espaços em branco.";
        var dto = new CreateClienteDto { Nome = "", Nascimento = new DateTime(1990, 1, 1) };

        // Act
        var result = await _useCase.Execute(dto);

        // Assert
        result.IsSuccess.Should().BeFalse();
        _repository.Count().Should().Be(0);
        result.Error.Should().Be(errorMessage);
         _logger.LogError($"Falha ao criar cliente: {result.Error}");
    }

    [Fact]
    public async Task Execute_NaoDeveSalvarClienteMenorIdade()
    {
        // Arrange
        var dto = new CreateClienteDto { Nome = "Joao", Nascimento = new DateTime(2020, 1, 1) };
        string errorMessage = "Cliente menor de idade";
        // Act
        var result = await _useCase.Execute(dto);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(errorMessage);
        _repository.Count().Should().Be(0);
        _logger.LogError($"Falha ao criar cliente: {result.Error}");
    }
}
