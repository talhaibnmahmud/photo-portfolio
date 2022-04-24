using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoTest.Migrations
{
    public partial class PostFavoriteCountAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FavoriteCount",
                table: "Post",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FavoriteCount",
                table: "Post");
        }
    }
}
