using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TadesApi.Db.Migrations
{
    /// <inheritdoc />
    public partial class AccounterUserUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AccounterUsers",
                newName: "TargetUserUserId");

            migrationBuilder.AddColumn<long>(
                name: "AccounterUserId",
                table: "AccounterUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccounterUserId",
                table: "AccounterUsers");

            migrationBuilder.RenameColumn(
                name: "TargetUserUserId",
                table: "AccounterUsers",
                newName: "UserId");
        }
    }
}
