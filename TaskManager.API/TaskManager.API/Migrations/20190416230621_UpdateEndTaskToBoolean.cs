using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace TaskManager.API.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class UpdateEndTaskToBoolean : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "EndTask",
                table: "Task",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EndTask",
                table: "Task",
                nullable: false,
                oldClrType: typeof(bool));
        }
    }
}
