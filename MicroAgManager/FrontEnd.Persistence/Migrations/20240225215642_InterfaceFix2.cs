using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InterfaceFix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LandPlots_AreaUnitId",
                table: "LandPlots",
                column: "AreaUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_LandPlots_Units_AreaUnitId",
                table: "LandPlots",
                column: "AreaUnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LandPlots_Units_AreaUnitId",
                table: "LandPlots");

            migrationBuilder.DropIndex(
                name: "IX_LandPlots_AreaUnitId",
                table: "LandPlots");
        }
    }
}
