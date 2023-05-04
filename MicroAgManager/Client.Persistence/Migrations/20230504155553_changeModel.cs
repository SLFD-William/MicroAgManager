using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class changeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LandPlots_Farms_FarmLocationModelId",
                table: "LandPlots");

            migrationBuilder.DropForeignKey(
                name: "FK_LandPlots_LandPlots_LandPlotModelId",
                table: "LandPlots");

            migrationBuilder.DropIndex(
                name: "IX_LandPlots_FarmLocationModelId",
                table: "LandPlots");

            migrationBuilder.DropIndex(
                name: "IX_LandPlots_LandPlotModelId",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "FarmLocationModelId",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "LandPlotModelId",
                table: "LandPlots");

            migrationBuilder.AlterColumn<string>(
                name: "AreaUnit",
                table: "LandPlots",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_LandPlots_FarmLocationId",
                table: "LandPlots",
                column: "FarmLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_LandPlots_ParentPlotId",
                table: "LandPlots",
                column: "ParentPlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_LandPlots_Farms_FarmLocationId",
                table: "LandPlots",
                column: "FarmLocationId",
                principalTable: "Farms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LandPlots_LandPlots_ParentPlotId",
                table: "LandPlots",
                column: "ParentPlotId",
                principalTable: "LandPlots",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LandPlots_Farms_FarmLocationId",
                table: "LandPlots");

            migrationBuilder.DropForeignKey(
                name: "FK_LandPlots_LandPlots_ParentPlotId",
                table: "LandPlots");

            migrationBuilder.DropIndex(
                name: "IX_LandPlots_FarmLocationId",
                table: "LandPlots");

            migrationBuilder.DropIndex(
                name: "IX_LandPlots_ParentPlotId",
                table: "LandPlots");

            migrationBuilder.AlterColumn<long>(
                name: "AreaUnit",
                table: "LandPlots",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<long>(
                name: "FarmLocationModelId",
                table: "LandPlots",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LandPlotModelId",
                table: "LandPlots",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandPlots_FarmLocationModelId",
                table: "LandPlots",
                column: "FarmLocationModelId");

            migrationBuilder.CreateIndex(
                name: "IX_LandPlots_LandPlotModelId",
                table: "LandPlots",
                column: "LandPlotModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_LandPlots_Farms_FarmLocationModelId",
                table: "LandPlots",
                column: "FarmLocationModelId",
                principalTable: "Farms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LandPlots_LandPlots_LandPlotModelId",
                table: "LandPlots",
                column: "LandPlotModelId",
                principalTable: "LandPlots",
                principalColumn: "Id");
        }
    }
}
