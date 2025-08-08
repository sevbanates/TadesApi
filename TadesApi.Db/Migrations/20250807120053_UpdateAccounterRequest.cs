using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TadesApi.Db.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccounterRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ModDate",
                table: "AccounterRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModDate",
                table: "AccounterRequests");
        }
    }
}
