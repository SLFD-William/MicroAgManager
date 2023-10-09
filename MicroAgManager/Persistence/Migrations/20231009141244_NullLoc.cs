using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NullLoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livestocks_Plots_LocationId",
                table: "Livestocks");

            migrationBuilder.AlterColumn<long>(
                name: "LocationId",
                table: "Livestocks",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Livestocks_Plots_LocationId",
                table: "Livestocks",
                column: "LocationId",
                principalTable: "Plots",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livestocks_Plots_LocationId",
                table: "Livestocks");

            migrationBuilder.AlterColumn<long>(
                name: "LocationId",
                table: "Livestocks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Livestocks_Plots_LocationId",
                table: "Livestocks",
                column: "LocationId",
                principalTable: "Plots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
