using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineVotingSystem.Migrations
{
    public partial class removeVoterFacedId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaceId",
                table: "Voter");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FaceId",
                table: "Voter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
