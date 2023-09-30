using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ScheduledDuty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livestocks_Plots_LocationId",
                table: "Livestocks");

            migrationBuilder.AddColumn<string>(
                name: "Recipient",
                table: "ScheduledDuties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "RecipientId",
                table: "ScheduledDuties",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Record",
                table: "ScheduledDuties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "RecordId",
                table: "ScheduledDuties",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livestocks_Plots_LocationId",
                table: "Livestocks");

            migrationBuilder.DropColumn(
                name: "Recipient",
                table: "ScheduledDuties");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "ScheduledDuties");

            migrationBuilder.DropColumn(
                name: "Record",
                table: "ScheduledDuties");

            migrationBuilder.DropColumn(
                name: "RecordId",
                table: "ScheduledDuties");

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
    }
}
