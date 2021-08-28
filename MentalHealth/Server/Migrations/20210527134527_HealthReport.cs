using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MentalHealth.Server.Migrations
{
    public partial class HealthReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Chat",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "HealthStatus",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "Chat");

            migrationBuilder.RenameTable(
                name: "Chat",
                newName: "Chats");

            migrationBuilder.RenameColumn(
                name: "SurName",
                table: "AspNetUsers",
                newName: "Surname");

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceFee",
                table: "UserProfessions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SenderBId",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SenderAId",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chats",
                table: "Chats",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SessionRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HealthOfficerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ServiceFeePaid = table.Column<bool>(type: "bit", nullable: false),
                    DateStarted = table.Column<DateTime>(type: "datetimeoffset", nullable: false),
                    DateEnded = table.Column<DateTime>(type: "datetimeoffset", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionRecords_UserProfessions_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "UserProfessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientHealthRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SessionRecordId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetimeoffset", nullable: false),
                    HealthRecord = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientHealthRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientHealthRecords_SessionRecords_SessionRecordId",
                        column: x => x.SessionRecordId,
                        principalTable: "SessionRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientHealthRecords_SessionRecordId",
                table: "PatientHealthRecords",
                column: "SessionRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionRecords_ProfessionId",
                table: "SessionRecords",
                column: "ProfessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientHealthRecords");

            migrationBuilder.DropTable(
                name: "SessionRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chats",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "ServiceFee",
                table: "UserProfessions");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Chats");

            migrationBuilder.RenameTable(
                name: "Chats",
                newName: "Chat");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "AspNetUsers",
                newName: "SurName");

            migrationBuilder.AddColumn<string>(
                name: "HealthStatus",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "Chat",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SenderBId",
                table: "Chat",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SenderAId",
                table: "Chat",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Chat",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConversationId",
                table: "Chat",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chat",
                table: "Chat",
                column: "Id");
        }
    }
}
