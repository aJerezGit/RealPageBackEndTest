using Microsoft.EntityFrameworkCore.Migrations;

namespace webapiRealPage.Migrations
{
    public partial class AdminFlagonUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Admin",
                table: "ApplicationUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Admin",
                table: "ApplicationUsers");
        }
    }
}
