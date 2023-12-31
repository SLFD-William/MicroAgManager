using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddingChore3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Duties_Chores_ChoreId",
                table: "Duties");

            migrationBuilder.DropIndex(
                name: "IX_Duties_ChoreId",
                table: "Duties");

            migrationBuilder.DropColumn(
                name: "ChoreId",
                table: "Duties");

            migrationBuilder.CreateTable(
                name: "ChoreDuty",
                columns: table => new
                {
                    ChoresId = table.Column<long>(type: "bigint", nullable: false),
                    DutiesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChoreDuty", x => new { x.ChoresId, x.DutiesId });
                    table.ForeignKey(
                        name: "FK_ChoreDuty_Chores_ChoresId",
                        column: x => x.ChoresId,
                        principalTable: "Chores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChoreDuty_Duties_DutiesId",
                        column: x => x.DutiesId,
                        principalTable: "Duties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChoreDuty_DutiesId",
                table: "ChoreDuty",
                column: "DutiesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChoreDuty");

            migrationBuilder.AddColumn<long>(
                name: "ChoreId",
                table: "Duties",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Duties_ChoreId",
                table: "Duties",
                column: "ChoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Duties_Chores_ChoreId",
                table: "Duties",
                column: "ChoreId",
                principalTable: "Chores",
                principalColumn: "Id");
        }
    }
}
