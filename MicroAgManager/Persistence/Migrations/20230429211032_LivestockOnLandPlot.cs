using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class LivestockOnLandPlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LandPlotLivestock",
                columns: table => new
                {
                    LivestocksId = table.Column<long>(type: "bigint", nullable: false),
                    LocationsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandPlotLivestock", x => new { x.LivestocksId, x.LocationsId });
                    table.ForeignKey(
                        name: "FK_LandPlotLivestock_Livestocks_LivestocksId",
                        column: x => x.LivestocksId,
                        principalTable: "Livestocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction,onUpdate:ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LandPlotLivestock_Plots_LocationsId",
                        column: x => x.LocationsId,
                        principalTable: "Plots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction, onUpdate: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LandPlotLivestock_LocationsId",
                table: "LandPlotLivestock",
                column: "LocationsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LandPlotLivestock");
        }
    }
}
