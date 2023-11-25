using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class droppedNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LivestockBreeds_LivestockAnimals_LivestockAnimalModelId",
                table: "LivestockBreeds");

            migrationBuilder.DropForeignKey(
                name: "FK_Livestocks_LivestockBreeds_LivestockBreedModelId",
                table: "Livestocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Livestocks_LivestockStatuses_LivestockStatusModelId",
                table: "Livestocks");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledDuties_Duties_DutyModelId",
                table: "ScheduledDuties");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledDuties_DutyModelId",
                table: "ScheduledDuties");

            migrationBuilder.DropIndex(
                name: "IX_Livestocks_LivestockBreedModelId",
                table: "Livestocks");

            migrationBuilder.DropIndex(
                name: "IX_Livestocks_LivestockStatusModelId",
                table: "Livestocks");

            migrationBuilder.DropIndex(
                name: "IX_LivestockBreeds_LivestockAnimalModelId",
                table: "LivestockBreeds");

            migrationBuilder.DropColumn(
                name: "DutyModelId",
                table: "ScheduledDuties");

            migrationBuilder.DropColumn(
                name: "DutyName",
                table: "ScheduledDuties");

            migrationBuilder.DropColumn(
                name: "RecipientName",
                table: "ScheduledDuties");

            migrationBuilder.DropColumn(
                name: "RecordName",
                table: "ScheduledDuties");

            migrationBuilder.DropColumn(
                name: "LivestockBreedModelId",
                table: "Livestocks");

            migrationBuilder.DropColumn(
                name: "LivestockStatusModelId",
                table: "Livestocks");

            migrationBuilder.DropColumn(
                name: "LivestockAnimalModelId",
                table: "LivestockBreeds");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDuties_DutyId",
                table: "ScheduledDuties",
                column: "DutyId");

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

            migrationBuilder.CreateIndex(
                name: "IX_LivestockBreeds_LivestockAnimalId",
                table: "LivestockBreeds",
                column: "LivestockAnimalId");

            migrationBuilder.AddForeignKey(
                name: "FK_LivestockBreeds_LivestockAnimals_LivestockAnimalId",
                table: "LivestockBreeds",
                column: "LivestockAnimalId",
                principalTable: "LivestockAnimals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledDuties_Duties_DutyId",
                table: "ScheduledDuties",
                column: "DutyId",
                principalTable: "Duties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LivestockBreeds_LivestockAnimals_LivestockAnimalId",
                table: "LivestockBreeds");

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

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledDuties_Duties_DutyId",
                table: "ScheduledDuties");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledDuties_DutyId",
                table: "ScheduledDuties");

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

            migrationBuilder.DropIndex(
                name: "IX_LivestockBreeds_LivestockAnimalId",
                table: "LivestockBreeds");

            migrationBuilder.AddColumn<long>(
                name: "DutyModelId",
                table: "ScheduledDuties",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DutyName",
                table: "ScheduledDuties",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipientName",
                table: "ScheduledDuties",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecordName",
                table: "ScheduledDuties",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AddColumn<long>(
                name: "LivestockAnimalModelId",
                table: "LivestockBreeds",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDuties_DutyModelId",
                table: "ScheduledDuties",
                column: "DutyModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Livestocks_LivestockBreedModelId",
                table: "Livestocks",
                column: "LivestockBreedModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Livestocks_LivestockStatusModelId",
                table: "Livestocks",
                column: "LivestockStatusModelId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockBreeds_LivestockAnimalModelId",
                table: "LivestockBreeds",
                column: "LivestockAnimalModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_LivestockBreeds_LivestockAnimals_LivestockAnimalModelId",
                table: "LivestockBreeds",
                column: "LivestockAnimalModelId",
                principalTable: "LivestockAnimals",
                principalColumn: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledDuties_Duties_DutyModelId",
                table: "ScheduledDuties",
                column: "DutyModelId",
                principalTable: "Duties",
                principalColumn: "Id");
        }
    }
}
