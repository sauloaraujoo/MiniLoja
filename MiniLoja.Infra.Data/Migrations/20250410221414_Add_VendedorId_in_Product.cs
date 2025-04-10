using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniLoja.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_VendedorId_in_Product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VendedorId",
                table: "Produtos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_VendedorId",
                table: "Produtos",
                column: "VendedorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Vendedores_VendedorId",
                table: "Produtos",
                column: "VendedorId",
                principalTable: "Vendedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Vendedores_VendedorId",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_VendedorId",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "VendedorId",
                table: "Produtos");
        }
    }
}
