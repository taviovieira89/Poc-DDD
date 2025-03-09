using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging;
using PocDomain.Aggregate.Cliente;
using Xunit;

public class ObterClienteUseCaseTests
{
    private readonly Mock<IClienteMongoRepository> _repositoryMock;
    private readonly Mock<ILogger<ObterClienteUseCase>> _loggerMock;
    private readonly Mock<IObterClienteUseCase> _obteruseCase;

    public ObterClienteUseCaseTests()
    {
        _repositoryMock = new Mock<IClienteMongoRepository>();
        _loggerMock = new Mock<ILogger<ObterClienteUseCase>>();
        _obteruseCase = new Mock<IObterClienteUseCase>();
    }
    [Fact]
    public async Task Execute_DeveCriarClienteComSucesso()
    {
        // Arrange
        var dto = new ObterClienteDto { Nome = "João Silva", Nascimento = new DateTime(1990, 1, 1) };
        var clientResulteMock = Cliente.Create(new Name(dto.Nome), new BirthDate(dto.Nascimento));
        var clienteResult = clientResulteMock.Value;

        _obteruseCase.Setup(r => r.Execute(dto)).Returns(Task.CompletedTask);
        // Act
        Func<Task> action = async () => await _obteruseCase.Object.Execute(dto);

        // Assert
         await action.Should().NotThrowAsync();
         await action.Should().CompleteWithinAsync(TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task Execute_DeveRetornarErroSeCriacaoFalhar()
    {
        // Arrange
        string errorMessage = "Cliente menor de idade";
        string errorConcat = $"Falha ao criar cliente: {errorMessage}";
        var dto = new ObterClienteDto { Nome = "", Nascimento = new DateTime(1990, 1, 1) };
        //var clienteResult = Result<Guid>.Failure(errorMessage);
        _obteruseCase.Setup(uc => uc.Execute(dto)).ThrowsAsync(new ClienteException(errorMessage));

        // Act
        Func<Task> action = async () => await _obteruseCase.Object.Execute(dto);

        // Assert
        await action.Should().ThrowAsync<ClienteException>()
            .WithMessage(errorMessage);
    }

    [Fact]
    public async Task Execute_DeveRetornarErroSeUsuarioJaExiste()
    {
        // Arrange
        string errorMessage = "Cliente já cadastrado";
        var dto = new ObterClienteDto { Nome = "Joao", Nascimento = new DateTime(1990, 1, 1) };
        _obteruseCase.Setup(uc => uc.Execute(dto)).ThrowsAsync(new ClienteException(errorMessage));

        // Act
        Func<Task> action = async () => await _obteruseCase.Object.Execute(dto);

        // Assert
        await action.Should().ThrowAsync<ClienteException>()
             .WithMessage(errorMessage);
    }

    [Fact]
    public async Task Execute_DeveLancarClienteException_SeErroNoRepositorio()
    {
        // Arrange
        var dto = new ObterClienteDto { Nome = "Maria Silva", Nascimento = new DateTime(1985, 5, 20) };
        //var clienteMock = Cliente.Create(new Name(dto.Nome), new BirthDate(dto.Nascimento));

        _obteruseCase.Setup(uc => uc.Execute(dto)).ThrowsAsync(new ClienteException("Erro ao executar o caso de uso ObterClienteUseCase."));

        // Act
        Func<Task> action = async () => await _obteruseCase.Object.Execute(dto);

        // Assert
        await action.Should().ThrowAsync<ClienteException>()
            .WithMessage("Erro ao executar o caso de uso ObterClienteUseCase.");
    }
}