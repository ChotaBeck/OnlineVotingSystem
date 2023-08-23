using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineVotingSystem.Migrations
{
    public partial class modifingCandidate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalVotes",
                table: "Candidate",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalVotes",
                table: "Candidate");
        }
    }
}
