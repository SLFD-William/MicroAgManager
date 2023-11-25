using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class droppedNames5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LivestockStatuses_LivestockAnimals_LivestockAnimalModelId",
                table: "LivestockStatuses");

            migrationBuilder.DropIndex(
                name: "IX_LivestockStatuses_LivestockAnimalModelId",
                table: "LivestockStatuses");

            migrationBuilder.DropColumn(
                name: "LivestockAnimalModelId",
                table: "LivestockStatuses");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockStatuses_LivestockAnimalId",
                table: "LivestockStatuses",
                column: "LivestockAnimalId");

            migrationBuilder.AddForeignKey(
                name: "FK_LivestockStatuses_LivestockAnimals_LivestockAnimalId",
                table: "LivestockStatuses",
                column: "LivestockAnimalId",
                principalTable: "LivestockAnimals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LivestockStatuses_LivestockAnimals_LivestockAnimalId",
                table: "LivestockStatuses");

            migrationBuilder.DropIndex(
                name: "IX_LivestockStatuses_LivestockAnimalId",
                table: "LivestockStatuses");

            migrationBuilder.AddColumn<long>(
                name: "LivestockAnimalModelId",
                table: "LivestockStatuses",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LivestockStatuses_LivestockAnimalModelId",
                table: "LivestockStatuses",
                column: "LivestockAnimalModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_LivestockStatuses_LivestockAnimals_LivestockAnimalModelId",
                table: "LivestockStatuses",
                column: "LivestockAnimalModelId",
                principalTable: "LivestockAnimals",
                principalColumn: "Id");
        }
    }
}
