using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTimeStamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "TreatmentRecords");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "ScheduledDuties");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "Registrars");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "Plots");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "Milestones");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "Measures");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "LivestockStatuses");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "Livestocks");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "LivestockFeedServings");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "LivestockFeeds");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "LivestockFeedDistributions");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "LivestockFeedAnalysisResults");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "LivestockFeedAnalysisParameters");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "LivestockFeedAnalyses");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "LivestockBreeds");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "LivestockAnimals");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "Duties");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "BreedingRecords");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "Units",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "Treatments",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "TreatmentRecords",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "ScheduledDuties",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "Registrations",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "Registrars",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "Plots",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "Milestones",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "Measures",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "Measurements",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "LivestockStatuses",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "Livestocks",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "LivestockFeedServings",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "LivestockFeeds",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "LivestockFeedDistributions",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "LivestockFeedAnalysisResults",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "LivestockFeedAnalysisParameters",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "LivestockFeedAnalyses",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "LivestockBreeds",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "LivestockAnimals",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "Farms",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "Events",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "Duties",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "BreedingRecords",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
