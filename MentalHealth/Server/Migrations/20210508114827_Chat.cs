using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MentalHealth.Server.Migrations
{
    public partial class Chat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConversationId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    SenderAId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    SenderBId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    DateSent = table.Column<DateTime>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chat");
        }
    }
}
