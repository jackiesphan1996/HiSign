using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSign.Infrastructure.Persistence.Migrations
{
    public partial class AddStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "User",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Customer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ContractType",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Company",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ContractType");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Company");
        }
    }
}
