using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProselApp.Migrations
{
    public partial class ProselV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Cpf = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    Telephone = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    AccountStatus = table.Column<bool>(nullable: false),
                    Receive_emails = table.Column<bool>(nullable: false),
                    AccessType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Cpf);
                });

            migrationBuilder.CreateTable(
                name: "AccessCode",
                columns: table => new
                {
                    Code = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserCpf = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: false),
                    CodeType = table.Column<string>(nullable: true),
                    GenDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessCode", x => x.Code);
                    table.ForeignKey(
                        name: "FK_AccessCode_User_UserCpf",
                        column: x => x.UserCpf,
                        principalTable: "User",
                        principalColumn: "Cpf",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Messagecode = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sender = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Telephone = table.Column<string>(nullable: false),
                    UserCpf = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Messagecode);
                    table.ForeignKey(
                        name: "FK_Message_User_UserCpf",
                        column: x => x.UserCpf,
                        principalTable: "User",
                        principalColumn: "Cpf",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessCode_UserCpf",
                table: "AccessCode",
                column: "UserCpf");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserCpf",
                table: "Message",
                column: "UserCpf");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessCode");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
