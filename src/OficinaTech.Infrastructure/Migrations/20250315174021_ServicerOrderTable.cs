using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OficinaTech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ServicerOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "orcamentos",
                type: "numeric(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "service_orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OrcamentoId = table.Column<int>(type: "integer", nullable: false),
                    StatusServiceOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_service_orders_orcamentos_OrcamentoId",
                        column: x => x.OrcamentoId,
                        principalTable: "orcamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_service_orders_Number",
                table: "service_orders",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_service_orders_OrcamentoId",
                table: "service_orders",
                column: "OrcamentoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "service_orders");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "orcamentos");
        }
    }
}
