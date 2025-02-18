using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PocInfra.Migrations
{
    /// <inheritdoc />
    public partial class ColunaName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes");

            migrationBuilder.RenameColumn(
                name: "Nome_Value",
                table: "Clientes",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "Nascimento_Value",
                table: "Clientes",
                newName: "Nascimento");

            migrationBuilder.AddPrimaryKey(
                name: "ClienteId",
                table: "Clientes",
                column: "IdCliente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "ClienteId",
                table: "Clientes");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Clientes",
                newName: "Nome_Value");

            migrationBuilder.RenameColumn(
                name: "Nascimento",
                table: "Clientes",
                newName: "Nascimento_Value");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes",
                column: "IdCliente");
        }
    }
}
