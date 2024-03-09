using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Treatments3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentRecords_Units_DosageUnitId",
                table: "TreatmentRecords");

            migrationBuilder.AlterColumn<long>(
                name: "DosageUnitId",
                table: "TreatmentRecords",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentRecords_Units_DosageUnitId",
                table: "TreatmentRecords",
                column: "DosageUnitId",
                principalTable: "Units",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentRecords_Units_DosageUnitId",
                table: "TreatmentRecords");

            migrationBuilder.AlterColumn<long>(
                name: "DosageUnitId",
                table: "TreatmentRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentRecords_Units_DosageUnitId",
                table: "TreatmentRecords",
                column: "DosageUnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
