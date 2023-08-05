using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLogging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "User",
                table: "Logs",
                newName: "EventName");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Logs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Logs");

            migrationBuilder.RenameColumn(
                name: "EventName",
                table: "Logs",
                newName: "User");
        }
    }
}
