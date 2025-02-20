using Xunit;
using PocDomain.Aggregate.Cliente;

namespace PocTests
{
    public class ClienteTests
    {
        [Fact]
        public void Nome_Deve_Lancar_Excecao_Se_Vazio()
        {
            Assert.Throws<ArgumentException>(() => new Name(""));
        }

        [Fact]
        public void Nome_Deve_Ser_Valido()
        {
            var nome = new Name("João Silva");
            Assert.Equal("João Silva", nome.Value);
        }

        [Fact]
        public void Cliente_Deve_Ser_Maior_Idade()
        {
            var clienteResult = Cliente.Create(new Name("João Silva"), new BirthDate(DateTime.Today.AddYears(-20)));
            var cliente = clienteResult.Value;

            Assert.True(cliente.MaiorIdade());
        }
    }
}
