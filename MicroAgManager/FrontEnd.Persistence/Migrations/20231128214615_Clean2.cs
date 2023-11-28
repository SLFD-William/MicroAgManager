using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Clean2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livestocks_LandPlots_LandPlotModelId",
                table: "Livestocks");

            migrationBuilder.DropIndex(
                name: "IX_Livestocks_LandPlotModelId",
                table: "Livestocks");

            migrationBuilder.DropColumn(
                name: "LandPlotModelId",
                table: "Livestocks");

            migrationBuilder.CreateIndex(
                name: "IX_Livestocks_LocationId",
                table: "Livestocks",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Livestocks_LandPlots_LocationId",
                table: "Livestocks",
                column: "LocationId",
                principalTable: "LandPlots",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livestocks_LandPlots_LocationId",
                table: "Livestocks");

            migrationBuilder.DropIndex(
                name: "IX_Livestocks_LocationId",
                table: "Livestocks");

            migrationBuilder.AddColumn<long>(
                name: "LandPlotModelId",
                table: "Livestocks",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Livestocks_LandPlotModelId",
                table: "Livestocks",
                column: "LandPlotModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Livestocks_LandPlots_LandPlotModelId",
                table: "Livestocks",
                column: "LandPlotModelId",
                principalTable: "LandPlots",
                principalColumn: "Id");
        }
    }
}
