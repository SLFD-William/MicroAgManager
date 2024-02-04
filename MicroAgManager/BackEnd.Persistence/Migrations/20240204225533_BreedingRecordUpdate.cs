using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BreedingRecordUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BreedingRecords_Livestocks_FemaleId",
                table: "BreedingRecords");

            migrationBuilder.DropIndex(
                name: "IX_BreedingRecords_FemaleId",
                table: "BreedingRecords");

            migrationBuilder.RenameColumn(
                name: "FemaleId",
                table: "BreedingRecords",
                newName: "RecipientTypeId");

            migrationBuilder.AddColumn<long>(
                name: "RecipientId",
                table: "BreedingRecords",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "RecipientType",
                table: "BreedingRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BreedingRecords_RecipientId",
                table: "BreedingRecords",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_BreedingRecords_Livestocks_RecipientId",
                table: "BreedingRecords",
                column: "RecipientId",
                principalTable: "Livestocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BreedingRecords_Livestocks_RecipientId",
                table: "BreedingRecords");

            migrationBuilder.DropIndex(
                name: "IX_BreedingRecords_RecipientId",
                table: "BreedingRecords");

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

            migrationBuilder.CreateIndex(
                name: "IX_BreedingRecords_FemaleId",
                table: "BreedingRecords",
                column: "FemaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_BreedingRecords_Livestocks_FemaleId",
                table: "BreedingRecords",
                column: "FemaleId",
                principalTable: "Livestocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
