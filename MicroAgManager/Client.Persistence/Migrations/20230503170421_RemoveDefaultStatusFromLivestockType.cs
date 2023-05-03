using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDefaultStatusFromLivestockType : Migration
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

            migrationBuilder.DropForeignKey(
                name: "FK_LivestockStatuses_LivestockTypes_LivestockTypeModelId",
                table: "LivestockStatuses");

            migrationBuilder.DropIndex(
                name: "IX_LivestockStatuses_LivestockTypeModelId",
                table: "LivestockStatuses");

            migrationBuilder.DropIndex(
                name: "IX_LandPlots_FarmLocationModelId",
                table: "LandPlots");

            migrationBuilder.DropIndex(
                name: "IX_LandPlots_LandPlotModelId",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "DefaultStatus",
                table: "LivestockTypes");

            migrationBuilder.DropColumn(
                name: "LivestockTypeModelId",
                table: "LivestockStatuses");

            migrationBuilder.DropColumn(
                name: "FarmLocationModelId",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "LandPlotModelId",
                table: "LandPlots");

            migrationBuilder.AddColumn<bool>(
                name: "DefaultStatus",
                table: "LivestockStatuses",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "FarmId",
                table: "LandPlots",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_LivestockStatuses_LivestockTypeId",
                table: "LivestockStatuses",
                column: "LivestockTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LandPlots_FarmId",
                table: "LandPlots",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_LandPlots_ParentPlotId",
                table: "LandPlots",
                column: "ParentPlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_LandPlots_Farms_FarmId",
                table: "LandPlots",
                column: "FarmId",
                principalTable: "Farms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LandPlots_LandPlots_ParentPlotId",
                table: "LandPlots",
                column: "ParentPlotId",
                principalTable: "LandPlots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LivestockStatuses_LivestockTypes_LivestockTypeId",
                table: "LivestockStatuses",
                column: "LivestockTypeId",
                principalTable: "LivestockTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LandPlots_Farms_FarmId",
                table: "LandPlots");

            migrationBuilder.DropForeignKey(
                name: "FK_LandPlots_LandPlots_ParentPlotId",
                table: "LandPlots");

            migrationBuilder.DropForeignKey(
                name: "FK_LivestockStatuses_LivestockTypes_LivestockTypeId",
                table: "LivestockStatuses");

            migrationBuilder.DropIndex(
                name: "IX_LivestockStatuses_LivestockTypeId",
                table: "LivestockStatuses");

            migrationBuilder.DropIndex(
                name: "IX_LandPlots_FarmId",
                table: "LandPlots");

            migrationBuilder.DropIndex(
                name: "IX_LandPlots_ParentPlotId",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "DefaultStatus",
                table: "LivestockStatuses");

            migrationBuilder.DropColumn(
                name: "FarmId",
                table: "LandPlots");

            migrationBuilder.AddColumn<string>(
                name: "DefaultStatus",
                table: "LivestockTypes",
                type: "TEXT",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "LivestockTypeModelId",
                table: "LivestockStatuses",
                type: "INTEGER",
                nullable: true);

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
                name: "IX_LivestockStatuses_LivestockTypeModelId",
                table: "LivestockStatuses",
                column: "LivestockTypeModelId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_LivestockStatuses_LivestockTypes_LivestockTypeModelId",
                table: "LivestockStatuses",
                column: "LivestockTypeModelId",
                principalTable: "LivestockTypes",
                principalColumn: "Id");
        }
    }
}
