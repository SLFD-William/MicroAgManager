using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDefaultStatusFromLivestockType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plots_Plots_LandPlotId",
                table: "Plots");

            migrationBuilder.DropIndex(
                name: "IX_Plots_LandPlotId",
                table: "Plots");

            migrationBuilder.DropColumn(
                name: "LandPlotId",
                table: "Plots");

            migrationBuilder.DropColumn(
                name: "DefaultStatus",
                table: "LivestockTypes");

            migrationBuilder.AddColumn<bool>(
                name: "DefaultStatus",
                table: "LivestockStatuses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Plots_ParentPlotId",
                table: "Plots",
                column: "ParentPlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plots_Plots_ParentPlotId",
                table: "Plots",
                column: "ParentPlotId",
                principalTable: "Plots",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plots_Plots_ParentPlotId",
                table: "Plots");

            migrationBuilder.DropIndex(
                name: "IX_Plots_ParentPlotId",
                table: "Plots");

            migrationBuilder.DropColumn(
                name: "DefaultStatus",
                table: "LivestockStatuses");

            migrationBuilder.AddColumn<long>(
                name: "LandPlotId",
                table: "Plots",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DefaultStatus",
                table: "LivestockTypes",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Plots_LandPlotId",
                table: "Plots",
                column: "LandPlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plots_Plots_LandPlotId",
                table: "Plots",
                column: "LandPlotId",
                principalTable: "Plots",
                principalColumn: "Id");
        }
    }
}
