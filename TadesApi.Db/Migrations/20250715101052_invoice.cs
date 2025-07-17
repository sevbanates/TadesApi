using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TadesApi.Db.Migrations
{
    /// <inheritdoc />
    public partial class invoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Customers_CustomerId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_CustomerId",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Invoices",
                newName: "Scenario");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "Invoices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "InvoiceType",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxOffice",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telephone",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Vkn",
                table: "Invoices",
                type: "int",
                maxLength: 11,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "InvoiceItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<int>(
                name: "DiscountRate",
                table: "InvoiceItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "InvoiceItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "InvoiceItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UnitType",
                table: "InvoiceItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VatRate",
                table: "InvoiceItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "District",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "InvoiceType",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TaxOffice",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Telephone",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Vkn",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "DiscountRate",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "UnitType",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "VatRate",
                table: "InvoiceItems");

            migrationBuilder.RenameColumn(
                name: "Scenario",
                table: "Invoices",
                newName: "CustomerId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "InvoiceItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CustomerId",
                table: "Invoices",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Customers_CustomerId",
                table: "Invoices",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
