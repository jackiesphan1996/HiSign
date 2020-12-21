using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSign.Infrastructure.Persistence.Migrations
{
    public partial class AddContentInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AInformation",
                table: "Contract",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BInformation",
                table: "Contract",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContractLaw",
                table: "Contract",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContractValue",
                table: "Contract",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Footer",
                table: "Contract",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Header",
                table: "Contract",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AInformation",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "BInformation",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "ContractLaw",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "ContractValue",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "Footer",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "Header",
                table: "Contract");
        }
    }
}
