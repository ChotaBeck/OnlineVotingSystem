using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineVotingSystem.Migrations
{
    public partial class changingvotemodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_Votes_VoteId",
                table: "Candidate");

            migrationBuilder.DropIndex(
                name: "IX_Candidate_VoteId",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "VoteId",
                table: "Candidate");

            migrationBuilder.AddColumn<int>(
                name: "CandidateId",
                table: "Votes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ElectionId",
                table: "Votes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CandidateId",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "ElectionId",
                table: "Votes");

            migrationBuilder.AddColumn<int>(
                name: "VoteId",
                table: "Candidate",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidate_VoteId",
                table: "Candidate",
                column: "VoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidate_Votes_VoteId",
                table: "Candidate",
                column: "VoteId",
                principalTable: "Votes",
                principalColumn: "Id");
        }
    }
}
