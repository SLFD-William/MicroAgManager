using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLivestockTypeToMilestone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LivestockTypeId",
                table: "Milestones",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LivestockTypeId",
                table: "Duties",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Milestones_LivestockTypeId",
                table: "Milestones",
                column: "LivestockTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Duties_LivestockTypeId",
                table: "Duties",
                column: "LivestockTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Duties_LivestockTypes_LivestockTypeId",
                table: "Duties",
                column: "LivestockTypeId",
                principalTable: "LivestockTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Milestones_LivestockTypes_LivestockTypeId",
                table: "Milestones",
                column: "LivestockTypeId",
                principalTable: "LivestockTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Duties_LivestockTypes_LivestockTypeId",
                table: "Duties");

            migrationBuilder.DropForeignKey(
                name: "FK_Milestones_LivestockTypes_LivestockTypeId",
                table: "Milestones");

            migrationBuilder.DropIndex(
                name: "IX_Milestones_LivestockTypeId",
                table: "Milestones");

            migrationBuilder.DropIndex(
                name: "IX_Duties_LivestockTypeId",
                table: "Duties");

            migrationBuilder.DropColumn(
                name: "LivestockTypeId",
                table: "Milestones");

            migrationBuilder.DropColumn(
                name: "LivestockTypeId",
                table: "Duties");
        }
    }
}
