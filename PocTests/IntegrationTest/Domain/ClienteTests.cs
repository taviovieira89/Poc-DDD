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
        // Arrange & Act
        Action act = () => new Name("");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("O nome não pode estar vazio ou conter apenas espaços em branco.");
    }

    [Fact]
    public void Create_DeveFalhar_SeDataDeNascimentoForFutura()
    {
        // Arrange & Act
        Action act = () => new BirthDate(DateTime.UtcNow.AddYears(1));
        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"A data de nascimento {DateTime.UtcNow.AddYears(1).ToString("dd/MM/yyyy")} não pode ser maior que a data atual.");
    }

    [Fact]
    public void Create_DeveFalhar_SeDataDeNascimentoForInvalida()
    {
        // Arrange & Act
        var dtnasc = DateTime.MinValue;
        Action act = () => new BirthDate(dtnasc);
        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"A data de nascimento {dtnasc.ToString("dd/MM/yyyy")} não pode ser inválida.");
    }


    [Fact]
    public void Create_DeveFalhar_SeNomeMuitoCurto()
    {
        // Arrange & Act
        Action act = () => new Name("A");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("O nome não pode ser muito curto.");
    }

    [Fact]
    public void Create_DeveFalhar_SeNomeMuitoLongo()
    {
        // Arrange & Act
        Action act = () => new Name(new string('A', 101));

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("O nome não pode ser muito longo.");
    }
}
