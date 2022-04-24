using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoTest.Migrations
{
    public partial class PostUpdateAddedComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Post_PostId",
                table: "Favorites");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Favorites",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Post_PostId",
                table: "Favorites",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Post_PostId",
                table: "Favorites");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Favorites",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Post_PostId",
                table: "Favorites",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
