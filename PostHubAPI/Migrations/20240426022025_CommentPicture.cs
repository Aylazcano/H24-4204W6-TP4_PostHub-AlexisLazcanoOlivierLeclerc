using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostHubAPI.Migrations
{
    public partial class CommentPicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "Pictures",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_CommentId",
                table: "Pictures",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Comments_CommentId",
                table: "Pictures",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Comments_CommentId",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_CommentId",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Pictures");
        }
    }
}
