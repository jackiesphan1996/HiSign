using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSign.Infrastructure.Persistence.Migrations
{
    public partial class AddPLHD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BelongToContractId",
                table: "Contract",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contract_BelongToContractId",
                table: "Contract",
                column: "BelongToContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_Contract_BelongToContractId",
                table: "Contract",
                column: "BelongToContractId",
                principalTable: "Contract",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contract_Contract_BelongToContractId",
                table: "Contract");

            migrationBuilder.DropIndex(
                name: "IX_Contract_BelongToContractId",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "BelongToContractId",
                table: "Contract");
        }
    }
}
