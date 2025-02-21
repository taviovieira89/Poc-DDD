using System;
using FluentAssertions;
using PocDomain.Aggregate.Cliente;
using Xunit;

public class ClienteTests
{
    [Fact]
    public void Create_DeveCriarClienteComNomeEDataValidos()
    {
        // Arrange
        var nome = new Name("João da Silva");
        var nascimento = new BirthDate(new DateTime(1990, 5, 15));

        // Act
        var result = Cliente.Create(nome, nascimento);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Nome.Value.Should().Be("João da Silva");
        result.Value.Nascimento.Value.Should().Be(new DateTime(1990, 5, 15));
    }

    [Fact]
    public void Create_DeveFalhar_SeNomeForVazio()
    {
        // Arrange
        var nome = new Name("");
        var nascimento = new BirthDate(new DateTime(1990, 5, 15));

        // Act
        var result = Cliente.Create(nome, nascimento);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("O nome não pode estar vazio ou conter apenas espaços em branco.");
    }

    [Fact]
    public void Create_DeveFalhar_SeDataDeNascimentoForFutura()
    {
        // Arrange
        var nome = new Name("Maria Souza");
        var nascimento = new BirthDate(DateTime.UtcNow.AddYears(1));

        // Act
        var result = Cliente.Create(nome, nascimento);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("A data de nascimento não pode ser maior que a data atual.");
    }

    [Fact]
    public void Create_DeveFalhar_SeNomeMuitoCurto()
    {
        // Arrange
        var nome = new Name("A");
        var nascimento = new BirthDate(new DateTime(1990, 5, 15));

        // Act
        var result = Cliente.Create(nome, nascimento);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("O nome não pode ser muito curto.");
    }

    [Fact]
    public void Create_DeveFalhar_SeNomeMuitoLongo()
    {
        // Arrange
        var nome = new Name(new string('A', 101)); // Nome com 100 caracteres
        var nascimento = new BirthDate(new DateTime(1990, 5, 15));

        // Act
        var result = Cliente.Create(nome, nascimento);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("O nome não pode ser muito longo.");
    }
}
