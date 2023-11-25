using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class droppedNames3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledDuties_Duties_DutyId",
                table: "ScheduledDuties");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledDuties_DutyId",
                table: "ScheduledDuties");

            migrationBuilder.AddColumn<long>(
                name: "DutyModelId",
                table: "ScheduledDuties",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDuties_DutyModelId",
                table: "ScheduledDuties",
                column: "DutyModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledDuties_Duties_DutyModelId",
                table: "ScheduledDuties",
                column: "DutyModelId",
                principalTable: "Duties",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledDuties_Duties_DutyModelId",
                table: "ScheduledDuties");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledDuties_DutyModelId",
                table: "ScheduledDuties");

            migrationBuilder.DropColumn(
                name: "DutyModelId",
                table: "ScheduledDuties");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDuties_DutyId",
                table: "ScheduledDuties",
                column: "DutyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledDuties_Duties_DutyId",
                table: "ScheduledDuties",
                column: "DutyId",
                principalTable: "Duties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
