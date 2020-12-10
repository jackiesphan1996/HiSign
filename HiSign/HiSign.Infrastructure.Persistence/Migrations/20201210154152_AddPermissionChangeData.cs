using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSign.Infrastructure.Persistence.Migrations
{
    public partial class AddPermissionChangeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermission_User_UserId1",
                table: "UserPermission");

            migrationBuilder.DropIndex(
                name: "IX_UserPermission_UserId1",
                table: "UserPermission");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserPermission");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserPermission",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermission_UserId",
                table: "UserPermission",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermission_User_UserId",
                table: "UserPermission",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermission_User_UserId",
                table: "UserPermission");

            migrationBuilder.DropIndex(
                name: "IX_UserPermission_UserId",
                table: "UserPermission");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserPermission",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "UserPermission",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPermission_UserId1",
                table: "UserPermission",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermission_User_UserId1",
                table: "UserPermission",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
