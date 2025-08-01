using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiapCloudGames.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AjustesEntidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PercentualDeDesconto",
                table: "Promocoes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PercentualDeDesconto",
                table: "Promocoes",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
