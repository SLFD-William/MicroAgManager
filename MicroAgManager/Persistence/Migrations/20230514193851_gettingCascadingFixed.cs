using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class gettingCascadingFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LivestockFeedServings_LivestockStatuses_LivestockStatusId",
                table: "LivestockFeedServings");

            migrationBuilder.CreateTable(
                name: "LivestockLivestockStatus",
                columns: table => new
                {
                    LivestocksId = table.Column<long>(type: "bigint", nullable: false),
                    StatusesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockLivestockStatus", x => new { x.LivestocksId, x.StatusesId });
                    table.ForeignKey(
                        name: "FK_LivestockLivestockStatus_LivestockStatuses_StatusesId",
                        column: x => x.StatusesId,
                        principalTable: "LivestockStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction,
                        onUpdate:ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LivestockLivestockStatus_Livestocks_LivestocksId",
                        column: x => x.LivestocksId,
                        principalTable: "Livestocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction,
                        onUpdate: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LivestockLivestockStatus_StatusesId",
                table: "LivestockLivestockStatus",
                column: "StatusesId");

            migrationBuilder.AddForeignKey(
                name: "FK_LivestockFeedServings_LivestockStatuses_LivestockStatusId",
                table: "LivestockFeedServings",
                column: "LivestockStatusId",
                principalTable: "LivestockStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction,
                onUpdate: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LivestockFeedServings_LivestockStatuses_LivestockStatusId",
                table: "LivestockFeedServings");

            migrationBuilder.DropTable(
                name: "LivestockLivestockStatus");

            migrationBuilder.AddForeignKey(
                name: "FK_LivestockFeedServings_LivestockStatuses_LivestockStatusId",
                table: "LivestockFeedServings",
                column: "LivestockStatusId",
                principalTable: "LivestockStatuses",
                principalColumn: "Id");
        }
    }
}
