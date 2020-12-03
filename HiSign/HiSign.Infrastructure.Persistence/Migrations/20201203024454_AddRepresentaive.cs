using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSign.Infrastructure.Persistence.Migrations
{
    public partial class AddRepresentaive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Representaive",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Representaive",
                table: "Company",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Representaive",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Representaive",
                table: "Company");
        }
    }
}
