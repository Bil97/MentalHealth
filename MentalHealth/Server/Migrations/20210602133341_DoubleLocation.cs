using Microsoft.EntityFrameworkCore.Migrations;

namespace MentalHealth.Server.Migrations
{
    public partial class DoubleLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "LocationLongitude",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<double>(
                name: "LocationLatitude",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LocationLongitude",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "LocationLatitude",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
