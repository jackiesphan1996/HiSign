using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSign.Infrastructure.Persistence.Migrations
{
    public partial class AddFileUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "Contract",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "Contract");
        }
    }
}
