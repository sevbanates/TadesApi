using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TadesApi.Db.Migrations
{
    /// <inheritdoc />
    public partial class AccounterModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccounterRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<long>(type: "bigint", nullable: false),
                    TargetId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccounterRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccounterUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccounterUsers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccounterRequests");

            migrationBuilder.DropTable(
                name: "AccounterUsers");
        }
    }
}
