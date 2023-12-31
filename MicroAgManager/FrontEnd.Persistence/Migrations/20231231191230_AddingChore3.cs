using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddingChore3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Duties_Chores_ChoreModelId",
                table: "Duties");

            migrationBuilder.DropIndex(
                name: "IX_Duties_ChoreModelId",
                table: "Duties");

            migrationBuilder.DropColumn(
                name: "ChoreModelId",
                table: "Duties");

            migrationBuilder.CreateTable(
                name: "ChoreModelDutyModel",
                columns: table => new
                {
                    ChoresId = table.Column<long>(type: "INTEGER", nullable: false),
                    DutiesId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChoreModelDutyModel", x => new { x.ChoresId, x.DutiesId });
                    table.ForeignKey(
                        name: "FK_ChoreModelDutyModel_Chores_ChoresId",
                        column: x => x.ChoresId,
                        principalTable: "Chores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChoreModelDutyModel_Duties_DutiesId",
                        column: x => x.DutiesId,
                        principalTable: "Duties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChoreModelDutyModel_DutiesId",
                table: "ChoreModelDutyModel",
                column: "DutiesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChoreModelDutyModel");

            migrationBuilder.AddColumn<long>(
                name: "ChoreModelId",
                table: "Duties",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Duties_ChoreModelId",
                table: "Duties",
                column: "ChoreModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Duties_Chores_ChoreModelId",
                table: "Duties",
                column: "ChoreModelId",
                principalTable: "Chores",
                principalColumn: "Id");
        }
    }
}
