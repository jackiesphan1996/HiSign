using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSign.Infrastructure.Persistence.Migrations
{
    public partial class AddDigitalsignature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActivedDate",
                table: "Contract",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DigitalSignature",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNumber = table.Column<string>(nullable: true),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalSignature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DigitalSignature_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DigitalSignature_CompanyId",
                table: "DigitalSignature",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DigitalSignature");

            migrationBuilder.DropColumn(
                name: "ActivedDate",
                table: "Contract");
        }
    }
}
