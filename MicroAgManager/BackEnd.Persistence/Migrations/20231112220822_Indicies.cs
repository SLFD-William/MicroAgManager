using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Indicies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Recipient",
                table: "ScheduledDuties",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Units_TenantId",
                table: "Units",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_TenantId",
                table: "Treatments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentRecords_RecipientType_RecipientTypeId",
                table: "TreatmentRecords",
                columns: new[] { "RecipientType", "RecipientTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentRecords_TenantId",
                table: "TreatmentRecords",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_GuidId",
                table: "Tenants",
                column: "GuidId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDuties_Recipient_RecipientId",
                table: "ScheduledDuties",
                columns: new[] { "Recipient", "RecipientId" });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDuties_TenantId",
                table: "ScheduledDuties",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_RecipientType_RecipientTypeId",
                table: "Registrations",
                columns: new[] { "RecipientType", "RecipientTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_TenantId",
                table: "Registrations",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrars_TenantId",
                table: "Registrars",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Plots_TenantId",
                table: "Plots",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Milestones_RecipientType_RecipientTypeId",
                table: "Milestones",
                columns: new[] { "RecipientType", "RecipientTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Milestones_TenantId",
                table: "Milestones",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Measures_TenantId",
                table: "Measures",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_RecipientType_RecipientTypeId",
                table: "Measurements",
                columns: new[] { "RecipientType", "RecipientTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_TenantId",
                table: "Measurements",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockStatuses_TenantId",
                table: "LivestockStatuses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Livestocks_TenantId",
                table: "Livestocks",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeedServings_TenantId",
                table: "LivestockFeedServings",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeeds_TenantId",
                table: "LivestockFeeds",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeedDistributions_TenantId",
                table: "LivestockFeedDistributions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeedAnalysisResults_TenantId",
                table: "LivestockFeedAnalysisResults",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeedAnalysisParameters_TenantId",
                table: "LivestockFeedAnalysisParameters",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeedAnalyses_TenantId",
                table: "LivestockFeedAnalyses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockBreeds_TenantId",
                table: "LivestockBreeds",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockAnimals_TenantId",
                table: "LivestockAnimals",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_TenantId",
                table: "Farms",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_TenantId",
                table: "Events",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Duties_TenantId",
                table: "Duties",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BreedingRecords_TenantId",
                table: "BreedingRecords",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Units_TenantId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_TenantId",
                table: "Treatments");

            migrationBuilder.DropIndex(
                name: "IX_TreatmentRecords_RecipientType_RecipientTypeId",
                table: "TreatmentRecords");

            migrationBuilder.DropIndex(
                name: "IX_TreatmentRecords_TenantId",
                table: "TreatmentRecords");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_GuidId",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledDuties_Recipient_RecipientId",
                table: "ScheduledDuties");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledDuties_TenantId",
                table: "ScheduledDuties");

            migrationBuilder.DropIndex(
                name: "IX_Registrations_RecipientType_RecipientTypeId",
                table: "Registrations");

            migrationBuilder.DropIndex(
                name: "IX_Registrations_TenantId",
                table: "Registrations");

            migrationBuilder.DropIndex(
                name: "IX_Registrars_TenantId",
                table: "Registrars");

            migrationBuilder.DropIndex(
                name: "IX_Plots_TenantId",
                table: "Plots");

            migrationBuilder.DropIndex(
                name: "IX_Milestones_RecipientType_RecipientTypeId",
                table: "Milestones");

            migrationBuilder.DropIndex(
                name: "IX_Milestones_TenantId",
                table: "Milestones");

            migrationBuilder.DropIndex(
                name: "IX_Measures_TenantId",
                table: "Measures");

            migrationBuilder.DropIndex(
                name: "IX_Measurements_RecipientType_RecipientTypeId",
                table: "Measurements");

            migrationBuilder.DropIndex(
                name: "IX_Measurements_TenantId",
                table: "Measurements");

            migrationBuilder.DropIndex(
                name: "IX_LivestockStatuses_TenantId",
                table: "LivestockStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Livestocks_TenantId",
                table: "Livestocks");

            migrationBuilder.DropIndex(
                name: "IX_LivestockFeedServings_TenantId",
                table: "LivestockFeedServings");

            migrationBuilder.DropIndex(
                name: "IX_LivestockFeeds_TenantId",
                table: "LivestockFeeds");

            migrationBuilder.DropIndex(
                name: "IX_LivestockFeedDistributions_TenantId",
                table: "LivestockFeedDistributions");

            migrationBuilder.DropIndex(
                name: "IX_LivestockFeedAnalysisResults_TenantId",
                table: "LivestockFeedAnalysisResults");

            migrationBuilder.DropIndex(
                name: "IX_LivestockFeedAnalysisParameters_TenantId",
                table: "LivestockFeedAnalysisParameters");

            migrationBuilder.DropIndex(
                name: "IX_LivestockFeedAnalyses_TenantId",
                table: "LivestockFeedAnalyses");

            migrationBuilder.DropIndex(
                name: "IX_LivestockBreeds_TenantId",
                table: "LivestockBreeds");

            migrationBuilder.DropIndex(
                name: "IX_LivestockAnimals_TenantId",
                table: "LivestockAnimals");

            migrationBuilder.DropIndex(
                name: "IX_Farms_TenantId",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_Events_TenantId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Duties_TenantId",
                table: "Duties");

            migrationBuilder.DropIndex(
                name: "IX_BreedingRecords_TenantId",
                table: "BreedingRecords");

            migrationBuilder.AlterColumn<string>(
                name: "Recipient",
                table: "ScheduledDuties",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
