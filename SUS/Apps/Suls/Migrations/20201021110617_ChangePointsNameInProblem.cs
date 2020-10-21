using Microsoft.EntityFrameworkCore.Migrations;

namespace Suls.Migrations
{
    public partial class ChangePointsNameInProblem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Point",
                table: "Problems");

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Problems",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Points",
                table: "Problems");

            migrationBuilder.AddColumn<int>(
                name: "Point",
                table: "Problems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
