using Microsoft.EntityFrameworkCore.Migrations;

namespace MentalHealth.Server.Migrations
{
    public partial class RemoveSessionRecordForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_SessionRecords_SessionId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_SessionId",
                table: "Chats");

            migrationBuilder.AlterColumn<string>(
                name: "SessionId",
                table: "Chats",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SessionId",
                table: "Chats",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chats_SessionId",
                table: "Chats",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_SessionRecords_SessionId",
                table: "Chats",
                column: "SessionId",
                principalTable: "SessionRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
