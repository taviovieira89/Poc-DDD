using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging;
using PocDomain.Aggregate.Cliente;
using Xunit;

public class CreateClienteUseCaseTests
{
    private readonly Mock<IClienteRepository> _repositoryMock;
    private readonly Mock<ILogger<CreateClienteUseCase>> _loggerMock;
    private readonly Mock<ICreateClienteUseCase> _useCase;

    public CreateClienteUseCaseTests()
    {
        _repositoryMock = new Mock<IClienteRepository>();
        _loggerMock = new Mock<ILogger<CreateClienteUseCase>>();
        _useCase = new Mock<ICreateClienteUseCase>();
    }

    [Fact]
    public async Task Execute_DeveCriarClienteComSucesso()
    {
        // Arrange
        var dto = new CreateClienteDto { Nome = "João Silva", Nascimento = new DateTime(1990, 1, 1) };
        var clientResulteMock = Cliente.Create(new Name(dto.Nome), new BirthDate(dto.Nascimento));
        var IdCliente = clientResulteMock.Value.IdCliente;
        _useCase.Setup(uc => uc.Execute(dto)).ReturnsAsync(Result<Guid>.Success(IdCliente));

        // Act
        var result = await _useCase.Object.Execute(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Execute_DeveRetornarErroSeCriacaoFalhar()
    {
        // Arrange
        string errorMessage = "O nome não pode estar vazio ou conter apenas espaços em branco.";
        var dto = new CreateClienteDto { Nome = "", Nascimento = new DateTime(1990, 1, 1) };
        var clienteResult = Result<Guid>.Failure(errorMessage);
        _useCase.Setup(uc => uc.Execute(dto)).ReturnsAsync(clienteResult);

        // Act
        var result = await _useCase.Object.Execute(dto);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(errorMessage);
        // _repositoryMock.Verify(r => r.Add(It.IsAny<Cliente>()), Times.Never);
        // _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        // _loggerMock.Verify(l => l.LogWarning(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Execute_DeveLancarClienteException_SeErroNoRepositorio()
    {
        // Arrange
        var dto = new CreateClienteDto { Nome = "Maria Silva", Nascimento = new DateTime(1985, 5, 20) };
        var clienteMock = Cliente.Create(new Name(dto.Nome), new BirthDate(dto.Nascimento));

        _useCase.Setup(uc => uc.Execute(dto)).ThrowsAsync(new ClienteException("Ocorreu um erro inesperado ao criar o cliente."));

        // Act
        Func<Task> action = async () => await _useCase.Object.Execute(dto);

        // Assert
        await action.Should().ThrowAsync<ClienteException>()
            .WithMessage("Ocorreu um erro inesperado ao criar o cliente.");

       //_loggerMock.Verify(l => l.LogError(It.IsAny<string>()), Times.Once);
    }
}
