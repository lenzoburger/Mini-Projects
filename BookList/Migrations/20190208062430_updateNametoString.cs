using Microsoft.EntityFrameworkCore.Migrations;

namespace BookList.Migrations
{
    public partial class updateNametoString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Books",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Name",
                table: "Books",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
