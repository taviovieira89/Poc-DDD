using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using PocDomain.Aggregate.Cliente;
using Xunit;

public class ObterClienteUseCaseIntegrationTests
{
    private readonly FakeClienteMongoRepository _repository;
    private readonly ObterClienteUseCase _useCase;
    private readonly ILogger<ObterClienteUseCase> _logger;
    public ObterClienteUseCaseIntegrationTests()
    {
        _repository = new FakeClienteMongoRepository();
        _logger = new LoggerFactory().CreateLogger<ObterClienteUseCase>();

        _useCase = new ObterClienteUseCase(_logger,_repository );
    }

    [Fact]
    public async Task Execute_DeveCriarClienteComSucesso()
    {
        // Arrange
        var dto = new ObterClienteDto { Nome = "Maria", Nascimento = new DateTime(1985, 5, 20) };

        // Act
        Func<Task> action = async () => await _useCase.Execute(dto);

        // Assert
        await action.Should().NotThrowAsync();
        _repository.Count().Should().Be(1);
    }

    [Fact]
    public async Task Execute_NaoDeveSalvarClienteMenorIdade()
    {
        // Arrange
        var dto = new ObterClienteDto { Nome = "Joao", Nascimento = new DateTime(2020, 1, 1) };
        string errorMessage = "Cliente menor de idade";
        // Act
        Func<Task> action = async () => await _useCase.Execute(dto);

        // Assert
        _repository.Count().Should().Be(0);
        await action.Should().ThrowAsync<ClienteException>()
            .WithMessage(errorMessage);
    }

    [Fact]
    public async Task Execute_NaoDeveSalvarClienteJaCadastrado()
    {
        // Arrange
        var dto = new ObterClienteDto { Nome = "Maria", Nascimento = new DateTime(1985, 5, 20) };
        string errorMessage = "Cliente já cadastrado";
        Cliente cliente = Cliente.Create(new Name(dto.Nome), new BirthDate(dto.Nascimento)).Value;
        // Act
       _repository.Add(cliente);
        Func<Task> action = async () => await _useCase.Execute(dto);

        // Assert
        _repository.Count().Should().BeGreaterThan(0);
        await action.Should().ThrowAsync<ClienteException>()
            .WithMessage(errorMessage);
    }
}
