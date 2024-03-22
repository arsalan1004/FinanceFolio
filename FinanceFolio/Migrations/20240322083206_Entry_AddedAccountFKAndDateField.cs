using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceFolio.Migrations
{
    /// <inheritdoc />
    public partial class Entry_AddedAccountFKAndDateField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "accountId",
                table: "Entry",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "dateofEntry",
                table: "Entry",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateIndex(
                name: "IX_Entry_accountId",
                table: "Entry",
                column: "accountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entry_Account_accountId",
                table: "Entry",
                column: "accountId",
                principalTable: "Account",
                principalColumn: "accountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entry_Account_accountId",
                table: "Entry");

            migrationBuilder.DropIndex(
                name: "IX_Entry_accountId",
                table: "Entry");

            migrationBuilder.DropColumn(
                name: "accountId",
                table: "Entry");

            migrationBuilder.DropColumn(
                name: "dateofEntry",
                table: "Entry");
        }
    }
}
