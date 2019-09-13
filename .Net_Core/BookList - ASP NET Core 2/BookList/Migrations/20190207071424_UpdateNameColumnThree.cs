using Microsoft.EntityFrameworkCore.Migrations;

namespace BookList.Migrations
{
    public partial class UpdateNameColumnThree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Came",
                table: "Books",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Came",
                table: "Books");
        }
    }
}
