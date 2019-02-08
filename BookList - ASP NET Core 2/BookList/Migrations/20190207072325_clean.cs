using Microsoft.EntityFrameworkCore.Migrations;

namespace BookList.Migrations
{
    public partial class clean : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Came",
                table: "Books");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {           
        }
    }
}
