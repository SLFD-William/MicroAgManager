using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Indicies2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Units_ModifiedOn",
                table: "Units",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_ModifiedOn",
                table: "Treatments",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentRecords_ModifiedOn",
                table: "TreatmentRecords",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_ModifiedOn",
                table: "Tenants",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDuties_ModifiedOn",
                table: "ScheduledDuties",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_ModifiedOn",
                table: "Registrations",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Registrars_ModifiedOn",
                table: "Registrars",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Plots_ModifiedOn",
                table: "Plots",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Milestones_ModifiedOn",
                table: "Milestones",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Measures_ModifiedOn",
                table: "Measures",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_ModifiedOn",
                table: "Measurements",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockStatuses_ModifiedOn",
                table: "LivestockStatuses",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Livestocks_ModifiedOn",
                table: "Livestocks",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeedServings_ModifiedOn",
                table: "LivestockFeedServings",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeeds_ModifiedOn",
                table: "LivestockFeeds",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeedDistributions_ModifiedOn",
                table: "LivestockFeedDistributions",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeedAnalysisResults_ModifiedOn",
                table: "LivestockFeedAnalysisResults",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeedAnalysisParameters_ModifiedOn",
                table: "LivestockFeedAnalysisParameters",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeedAnalyses_ModifiedOn",
                table: "LivestockFeedAnalyses",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockBreeds_ModifiedOn",
                table: "LivestockBreeds",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockAnimals_ModifiedOn",
                table: "LivestockAnimals",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_ModifiedOn",
                table: "Farms",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Events_ModifiedOn",
                table: "Events",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Duties_ModifiedOn",
                table: "Duties",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_BreedingRecords_ModifiedOn",
                table: "BreedingRecords",
                column: "ModifiedOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Units_ModifiedOn",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_ModifiedOn",
                table: "Treatments");

            migrationBuilder.DropIndex(
                name: "IX_TreatmentRecords_ModifiedOn",
                table: "TreatmentRecords");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_ModifiedOn",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledDuties_ModifiedOn",
                table: "ScheduledDuties");

            migrationBuilder.DropIndex(
                name: "IX_Registrations_ModifiedOn",
                table: "Registrations");

            migrationBuilder.DropIndex(
                name: "IX_Registrars_ModifiedOn",
                table: "Registrars");

            migrationBuilder.DropIndex(
                name: "IX_Plots_ModifiedOn",
                table: "Plots");

            migrationBuilder.DropIndex(
                name: "IX_Milestones_ModifiedOn",
                table: "Milestones");

            migrationBuilder.DropIndex(
                name: "IX_Measures_ModifiedOn",
                table: "Measures");

            migrationBuilder.DropIndex(
                name: "IX_Measurements_ModifiedOn",
                table: "Measurements");

            migrationBuilder.DropIndex(
                name: "IX_LivestockStatuses_ModifiedOn",
                table: "LivestockStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Livestocks_ModifiedOn",
                table: "Livestocks");

            migrationBuilder.DropIndex(
                name: "IX_LivestockFeedServings_ModifiedOn",
                table: "LivestockFeedServings");

            migrationBuilder.DropIndex(
                name: "IX_LivestockFeeds_ModifiedOn",
                table: "LivestockFeeds");

            migrationBuilder.DropIndex(
                name: "IX_LivestockFeedDistributions_ModifiedOn",
                table: "LivestockFeedDistributions");

            migrationBuilder.DropIndex(
                name: "IX_LivestockFeedAnalysisResults_ModifiedOn",
                table: "LivestockFeedAnalysisResults");

            migrationBuilder.DropIndex(
                name: "IX_LivestockFeedAnalysisParameters_ModifiedOn",
                table: "LivestockFeedAnalysisParameters");

            migrationBuilder.DropIndex(
                name: "IX_LivestockFeedAnalyses_ModifiedOn",
                table: "LivestockFeedAnalyses");

            migrationBuilder.DropIndex(
                name: "IX_LivestockBreeds_ModifiedOn",
                table: "LivestockBreeds");

            migrationBuilder.DropIndex(
                name: "IX_LivestockAnimals_ModifiedOn",
                table: "LivestockAnimals");

            migrationBuilder.DropIndex(
                name: "IX_Farms_ModifiedOn",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_Events_ModifiedOn",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Duties_ModifiedOn",
                table: "Duties");

            migrationBuilder.DropIndex(
                name: "IX_BreedingRecords_ModifiedOn",
                table: "BreedingRecords");
        }
    }
}
