using Microsoft.EntityFrameworkCore.Migrations;

namespace Resume.Server.Migrations
{
    public partial class EditModel_PaymentReciept_AddedProperties_ExternalRef_InternalRef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalRef",
                table: "PaymentReceipt",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternalRef",
                table: "PaymentReceipt",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalRef",
                table: "PaymentReceipt");

            migrationBuilder.DropColumn(
                name: "InternalRef",
                table: "PaymentReceipt");
        }
    }
}
