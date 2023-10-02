using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DutyShakeup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LivestockAnimalId",
                table: "Milestones");

            migrationBuilder.DropColumn(
                name: "LivestockAnimalId",
                table: "Duties");

            migrationBuilder.AddColumn<string>(
                name: "RecipientType",
                table: "Milestones",
                type: "TEXT",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "RecipientTypeId",
                table: "Milestones",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "RecipientType",
                table: "Duties",
                type: "TEXT",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "RecipientTypeId",
                table: "Duties",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientType",
                table: "Milestones");

            migrationBuilder.DropColumn(
                name: "RecipientTypeId",
                table: "Milestones");

            migrationBuilder.DropColumn(
                name: "RecipientType",
                table: "Duties");

            migrationBuilder.DropColumn(
                name: "RecipientTypeId",
                table: "Duties");

            migrationBuilder.AddColumn<long>(
                name: "LivestockAnimalId",
                table: "Milestones",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LivestockAnimalId",
                table: "Duties",
                type: "INTEGER",
                nullable: true);
        }
    }
}
