using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineVotingSystem.Migrations
{
    public partial class changedVoteModel1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VoteId",
                table: "Candidate",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_Votes_VoteId",
                table: "Candidate");

            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Candidate_VoteId",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "VoteId",
                table: "Candidate");
        }
    }
}
