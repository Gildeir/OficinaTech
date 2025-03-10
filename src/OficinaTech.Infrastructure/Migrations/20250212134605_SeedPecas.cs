using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OficinaTech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedPecas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "pecas",
                columns: new[] { "Id", "Estoque", "Nome", "Preco" },
                values: new object[,]
                {
                    { -3, 8, "Correia Dentada", 75.30m },
                    { -2, 15, "Pastilha de Freio", 120.50m },
                    { -1, 10, "Filtro de Óleo", 25.90m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "pecas",
                keyColumn: "Id",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "pecas",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "pecas",
                keyColumn: "Id",
                keyValue: -1);
        }
    }
}
