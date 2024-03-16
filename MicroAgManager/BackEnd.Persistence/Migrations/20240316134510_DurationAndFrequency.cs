using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DurationAndFrequency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chores_Units_FrequencyUnitId",
                table: "Chores");

            migrationBuilder.DropForeignKey(
                name: "FK_Chores_Units_PeriodUnitId",
                table: "Chores");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_Units_FrequencyUnitId",
                table: "Treatments");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_FrequencyUnitId",
                table: "Treatments");

            migrationBuilder.DropIndex(
                name: "IX_Chores_PeriodUnitId",
                table: "Chores");

            migrationBuilder.DropColumn(
                name: "FrequencyUnitId",
                table: "Treatments");

            migrationBuilder.RenameColumn(
                name: "Frequency",
                table: "Treatments",
                newName: "PerScalar");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Treatments",
                newName: "EveryScalar");

            migrationBuilder.RenameColumn(
                name: "PeriodUnitId",
                table: "Chores",
                newName: "DurationUnitId");

            migrationBuilder.RenameColumn(
                name: "Period",
                table: "Chores",
                newName: "PerScalar");

            migrationBuilder.RenameColumn(
                name: "FrequencyUnitId",
                table: "Chores",
                newName: "PerUnitId");

            migrationBuilder.RenameColumn(
                name: "Frequency",
                table: "Chores",
                newName: "EveryScalar");

            migrationBuilder.RenameIndex(
                name: "IX_Chores_FrequencyUnitId",
                table: "Chores",
                newName: "IX_Chores_PerUnitId");

            migrationBuilder.AlterColumn<long>(
                name: "DurationUnitId",
                table: "Treatments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DurationScalar",
                table: "Treatments",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "EveryUnitId",
                table: "Treatments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PerUnitId",
                table: "Treatments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "DurationScalar",
                table: "Chores",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "EveryUnitId",
                table: "Chores",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_EveryUnitId",
                table: "Treatments",
                column: "EveryUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_PerUnitId",
                table: "Treatments",
                column: "PerUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Chores_EveryUnitId",
                table: "Chores",
                column: "EveryUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chores_Units_EveryUnitId",
                table: "Chores",
                column: "EveryUnitId",
                principalTable: "Units",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chores_Units_PerUnitId",
                table: "Chores",
                column: "PerUnitId",
                principalTable: "Units",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_Units_EveryUnitId",
                table: "Treatments",
                column: "EveryUnitId",
                principalTable: "Units",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_Units_PerUnitId",
                table: "Treatments",
                column: "PerUnitId",
                principalTable: "Units",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chores_Units_EveryUnitId",
                table: "Chores");

            migrationBuilder.DropForeignKey(
                name: "FK_Chores_Units_PerUnitId",
                table: "Chores");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_Units_EveryUnitId",
                table: "Treatments");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_Units_PerUnitId",
                table: "Treatments");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_EveryUnitId",
                table: "Treatments");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_PerUnitId",
                table: "Treatments");

            migrationBuilder.DropIndex(
                name: "IX_Chores_EveryUnitId",
                table: "Chores");

            migrationBuilder.DropColumn(
                name: "DurationScalar",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "EveryUnitId",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "PerUnitId",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "DurationScalar",
                table: "Chores");

            migrationBuilder.DropColumn(
                name: "EveryUnitId",
                table: "Chores");

            migrationBuilder.RenameColumn(
                name: "PerScalar",
                table: "Treatments",
                newName: "Frequency");

            migrationBuilder.RenameColumn(
                name: "EveryScalar",
                table: "Treatments",
                newName: "Duration");

            migrationBuilder.RenameColumn(
                name: "PerUnitId",
                table: "Chores",
                newName: "FrequencyUnitId");

            migrationBuilder.RenameColumn(
                name: "PerScalar",
                table: "Chores",
                newName: "Period");

            migrationBuilder.RenameColumn(
                name: "EveryScalar",
                table: "Chores",
                newName: "Frequency");

            migrationBuilder.RenameColumn(
                name: "DurationUnitId",
                table: "Chores",
                newName: "PeriodUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Chores_PerUnitId",
                table: "Chores",
                newName: "IX_Chores_FrequencyUnitId");

            migrationBuilder.AlterColumn<long>(
                name: "DurationUnitId",
                table: "Treatments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "FrequencyUnitId",
                table: "Treatments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_FrequencyUnitId",
                table: "Treatments",
                column: "FrequencyUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Chores_PeriodUnitId",
                table: "Chores",
                column: "PeriodUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chores_Units_FrequencyUnitId",
                table: "Chores",
                column: "FrequencyUnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chores_Units_PeriodUnitId",
                table: "Chores",
                column: "PeriodUnitId",
                principalTable: "Units",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_Units_FrequencyUnitId",
                table: "Treatments",
                column: "FrequencyUnitId",
                principalTable: "Units",
                principalColumn: "Id");
        }
    }
}
