using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class droppedNames2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livestocks_LivestockBreeds_LivestockBreedId",
                table: "Livestocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Livestocks_LivestockStatuses_StatusId",
                table: "Livestocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Livestocks_Livestocks_FatherId",
                table: "Livestocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Livestocks_Livestocks_MotherId",
                table: "Livestocks");

            migrationBuilder.DropIndex(
                name: "IX_Livestocks_FatherId",
                table: "Livestocks");

            migrationBuilder.DropIndex(
                name: "IX_Livestocks_LivestockBreedId",
                table: "Livestocks");

            migrationBuilder.DropIndex(
                name: "IX_Livestocks_MotherId",
                table: "Livestocks");

            migrationBuilder.DropIndex(
                name: "IX_Livestocks_StatusId",
                table: "Livestocks");

            migrationBuilder.AddColumn<long>(
                name: "LivestockBreedModelId",
                table: "Livestocks",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LivestockStatusModelId",
                table: "Livestocks",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Livestocks_LivestockBreedModelId",
                table: "Livestocks",
                column: "LivestockBreedModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Livestocks_LivestockStatusModelId",
                table: "Livestocks",
                column: "LivestockStatusModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Livestocks_LivestockBreeds_LivestockBreedModelId",
                table: "Livestocks",
                column: "LivestockBreedModelId",
                principalTable: "LivestockBreeds",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Livestocks_LivestockStatuses_LivestockStatusModelId",
                table: "Livestocks",
                column: "LivestockStatusModelId",
                principalTable: "LivestockStatuses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livestocks_LivestockBreeds_LivestockBreedModelId",
                table: "Livestocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Livestocks_LivestockStatuses_LivestockStatusModelId",
                table: "Livestocks");

            migrationBuilder.DropIndex(
                name: "IX_Livestocks_LivestockBreedModelId",
                table: "Livestocks");

            migrationBuilder.DropIndex(
                name: "IX_Livestocks_LivestockStatusModelId",
                table: "Livestocks");

            migrationBuilder.DropColumn(
                name: "LivestockBreedModelId",
                table: "Livestocks");

            migrationBuilder.DropColumn(
                name: "LivestockStatusModelId",
                table: "Livestocks");

            migrationBuilder.CreateIndex(
                name: "IX_Livestocks_FatherId",
                table: "Livestocks",
                column: "FatherId");

            migrationBuilder.CreateIndex(
                name: "IX_Livestocks_LivestockBreedId",
                table: "Livestocks",
                column: "LivestockBreedId");

            migrationBuilder.CreateIndex(
                name: "IX_Livestocks_MotherId",
                table: "Livestocks",
                column: "MotherId");

            migrationBuilder.CreateIndex(
                name: "IX_Livestocks_StatusId",
                table: "Livestocks",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Livestocks_LivestockBreeds_LivestockBreedId",
                table: "Livestocks",
                column: "LivestockBreedId",
                principalTable: "LivestockBreeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Livestocks_LivestockStatuses_StatusId",
                table: "Livestocks",
                column: "StatusId",
                principalTable: "LivestockStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Livestocks_Livestocks_FatherId",
                table: "Livestocks",
                column: "FatherId",
                principalTable: "Livestocks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Livestocks_Livestocks_MotherId",
                table: "Livestocks",
                column: "MotherId",
                principalTable: "Livestocks",
                principalColumn: "Id");
        }
    }
}
