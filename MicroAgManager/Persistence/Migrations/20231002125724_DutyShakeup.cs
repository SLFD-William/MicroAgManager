using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DutyShakeup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Duties_LivestockAnimals_LivestockAnimalId",
                table: "Duties");

            migrationBuilder.DropForeignKey(
                name: "FK_Milestones_LivestockAnimals_LivestockAnimalId",
                table: "Milestones");

            migrationBuilder.DropIndex(
                name: "IX_Milestones_LivestockAnimalId",
                table: "Milestones");

            migrationBuilder.DropIndex(
                name: "IX_Duties_LivestockAnimalId",
                table: "Duties");

            migrationBuilder.DropColumn(
                name: "LivestockAnimalId",
                table: "Milestones");

            migrationBuilder.DropColumn(
                name: "LivestockAnimalId",
                table: "Duties");

            migrationBuilder.AddColumn<string>(
                name: "RecipientType",
                table: "Milestones",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "RecipientTypeId",
                table: "Milestones",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "RecipientType",
                table: "Duties",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "RecipientTypeId",
                table: "Duties",
                type: "bigint",
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
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LivestockAnimalId",
                table: "Duties",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Milestones_LivestockAnimalId",
                table: "Milestones",
                column: "LivestockAnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Duties_LivestockAnimalId",
                table: "Duties",
                column: "LivestockAnimalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Duties_LivestockAnimals_LivestockAnimalId",
                table: "Duties",
                column: "LivestockAnimalId",
                principalTable: "LivestockAnimals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Milestones_LivestockAnimals_LivestockAnimalId",
                table: "Milestones",
                column: "LivestockAnimalId",
                principalTable: "LivestockAnimals",
                principalColumn: "Id");
        }
    }
}
