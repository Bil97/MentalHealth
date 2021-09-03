using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MentalHealth.Server.Migrations
{
    public partial class LatitudeLongitude : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "AspNetUsers",
                newName: "LocationLongitude");

            migrationBuilder.AddColumn<string>(
                name: "LocationLatitude",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MpesaAccounts",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccountBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MpesaAccounts", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "MpesaTransactions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    TransactionType = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Data = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MpesaTransactions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MpesaAccounts");

            migrationBuilder.DropTable(
                name: "MpesaTransactions");

            migrationBuilder.DropColumn(
                name: "LocationLatitude",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "LocationLongitude",
                table: "AspNetUsers",
                newName: "Location");
        }
    }
}
