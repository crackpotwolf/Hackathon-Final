using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class InnovationRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InnovationRequests",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Problem = table.Column<string>(type: "text", nullable: true),
                    ProblemDescription = table.Column<string>(type: "text", nullable: true),
                    ProblemEffects = table.Column<string>(type: "text", nullable: true),
                    ProblemReason = table.Column<string>(type: "text", nullable: true),
                    ProblemArea = table.Column<string>(type: "text", nullable: true),
                    ProblemTiming = table.Column<string>(type: "text", nullable: true),
                    SolvingAttempts = table.Column<string>(type: "text", nullable: true),
                    Contacts = table.Column<string>(type: "text", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InnovationRequests", x => x.Guid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InnovationRequests");
        }
    }
}
