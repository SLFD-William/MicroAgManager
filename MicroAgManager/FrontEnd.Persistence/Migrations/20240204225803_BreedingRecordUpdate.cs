using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BreedingRecordUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FemaleId",
                table: "BreedingRecords",
                newName: "RecipientTypeId");

            migrationBuilder.AddColumn<long>(
                name: "RecipientId",
                table: "BreedingRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "RecipientType",
                table: "BreedingRecords",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "BreedingRecords");

            migrationBuilder.DropColumn(
                name: "RecipientType",
                table: "BreedingRecords");

            migrationBuilder.RenameColumn(
                name: "RecipientTypeId",
                table: "BreedingRecords",
                newName: "FemaleId");
        }
    }
}
