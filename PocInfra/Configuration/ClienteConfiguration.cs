using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocDomain.Aggregate.Cliente;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        // Define a chave primária
        builder.HasKey(c => c.IdCliente);

        // Configura a propriedade Nome
        builder.OwnsOne(c => c.Nome, nome =>
        {
            nome.Property(n => n.Value)
            .IsRequired() 
            .HasMaxLength(100);  
        });

        // Configura a propriedade Nascimento como Value Object
        builder.OwnsOne(c => c.Nascimento, nascimento =>
        {
            nascimento.Property(n => n.Value)
                  .IsRequired()
                  .HasColumnType("datetime");  // Define o tipo da coluna no banco de dados
        });

        // // Configura a propriedade Email
        // builder.Property(c => c.Email)
        //        .IsRequired() // Obrigatório
        //        .HasMaxLength(100); // Tamanho máximo de 100 caracteres

        // // Configura a propriedade DataCadastro
        // builder.Property(c => c.DataCadastro)
        //        .IsRequired() // Obrigatório
        //        .HasDefaultValueSql("GETDATE()"); // Valor padrão no banco de dados

        // // Cria um índice único para o Email
        // builder.HasIndex(c => c.Email)
        //        .IsUnique(); // Garante que o email seja único
    }
}