using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TadesApi.Db.Migrations
{
    /// <inheritdoc />
    public partial class AccounterAndUserRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AccounterId",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequesterID = table.Column<long>(type: "bigint", nullable: false),
                    TargetUserID = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRequests_Users_RequesterID",
                        column: x => x.RequesterID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRequests_Users_TargetUserID",
                        column: x => x.TargetUserID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_AccounterId",
                table: "Users",
                column: "AccounterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRequests_RequesterID",
                table: "UserRequests",
                column: "RequesterID");

            migrationBuilder.CreateIndex(
                name: "IX_UserRequests_TargetUserID",
                table: "UserRequests",
                column: "TargetUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_AccounterId",
                table: "Users",
                column: "AccounterId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_AccounterId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserRequests");

            migrationBuilder.DropIndex(
                name: "IX_Users_AccounterId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AccounterId",
                table: "Users");
        }
    }
}
