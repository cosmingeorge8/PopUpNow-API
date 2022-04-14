using Microsoft.EntityFrameworkCore.Migrations;

namespace PopUp_Now_API.Migrations
{
    public partial class changedfavorites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Favorites",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId1",
                table: "Favorites",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_AspNetUsers_UserId1",
                table: "Favorites",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_AspNetUsers_UserId1",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_UserId1",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Favorites");
        }
    }
}
