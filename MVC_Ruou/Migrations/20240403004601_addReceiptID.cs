using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_Ruou.Migrations
{
    /// <inheritdoc />
    public partial class addReceiptID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiptID",
                table: "Receipt",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiptID",
                table: "Receipt");
        }
    }
}
