using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_Ruou.Migrations
{
    /// <inheritdoc />
    public partial class updateReceiptDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "inputPrice",
                table: "ReceiptDetail",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "outputPrice",
                table: "ReceiptDetail",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "inputPrice",
                table: "ReceiptDetail");

            migrationBuilder.DropColumn(
                name: "outputPrice",
                table: "ReceiptDetail");

        }
    }
}
