using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Resume.Server.Migrations
{
    public partial class AddModel_FootballTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FootballTeam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdateOfSeasonMatches = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SeasonIdForFootballMatches = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JSListOfFootBallMatch_Matches = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FootballTeam", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FootballTeam");
        }
    }
}
