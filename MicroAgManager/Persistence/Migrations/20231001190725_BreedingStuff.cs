using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BreedingStuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BornFemales",
                table: "BreedingRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BornMales",
                table: "BreedingRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Resolution",
                table: "BreedingRecords",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BornFemales",
                table: "BreedingRecords");

            migrationBuilder.DropColumn(
                name: "BornMales",
                table: "BreedingRecords");

            migrationBuilder.DropColumn(
                name: "Resolution",
                table: "BreedingRecords");
        }
    }
}
