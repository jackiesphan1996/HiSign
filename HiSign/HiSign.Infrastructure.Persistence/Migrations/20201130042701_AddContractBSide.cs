using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSign.Infrastructure.Persistence.Migrations
{
    public partial class AddContractBSide : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerCompanyId",
                table: "Contract");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Contract",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContractNum",
                table: "Contract",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContractPlace",
                table: "Contract",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Contract",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Contract",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Contract",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contract_CustomerId",
                table: "Contract",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_Customer_CustomerId",
                table: "Contract",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contract_Customer_CustomerId",
                table: "Contract");

            migrationBuilder.DropIndex(
                name: "IX_Contract_CustomerId",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "ContractNum",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "ContractPlace",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Contract");

            migrationBuilder.AddColumn<int>(
                name: "CustomerCompanyId",
                table: "Contract",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
