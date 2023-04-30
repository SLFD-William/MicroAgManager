using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class LivestockOnLandPlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LandPlotModelLivestockModel",
                columns: table => new
                {
                    LivestocksId = table.Column<long>(type: "INTEGER", nullable: false),
                    LocationsId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandPlotModelLivestockModel", x => new { x.LivestocksId, x.LocationsId });
                    table.ForeignKey(
                        name: "FK_LandPlotModelLivestockModel_LandPlots_LocationsId",
                        column: x => x.LocationsId,
                        principalTable: "LandPlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction,onUpdate:ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LandPlotModelLivestockModel_Livestocks_LivestocksId",
                        column: x => x.LivestocksId,
                        principalTable: "Livestocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction, onUpdate: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LandPlotModelLivestockModel_LocationsId",
                table: "LandPlotModelLivestockModel",
                column: "LocationsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LandPlotModelLivestockModel");
        }
    }
}
