using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InterfaceFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "BreedingRecords");

            migrationBuilder.AddColumn<long>(
                name: "EventModelId",
                table: "ScheduledDuties",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LivestockAnimalModelId",
                table: "LivestockFeeds",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChoreModelDutyModel",
                columns: table => new
                {
                    ChoresId = table.Column<long>(type: "INTEGER", nullable: false),
                    DutiesId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChoreModelDutyModel", x => new { x.ChoresId, x.DutiesId });
                    table.ForeignKey(
                        name: "FK_ChoreModelDutyModel_Chores_ChoresId",
                        column: x => x.ChoresId,
                        principalTable: "Chores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChoreModelDutyModel_Duties_DutiesId",
                        column: x => x.DutiesId,
                        principalTable: "Duties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DutyModelEventModel",
                columns: table => new
                {
                    DutiesId = table.Column<long>(type: "INTEGER", nullable: false),
                    EventsId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DutyModelEventModel", x => new { x.DutiesId, x.EventsId });
                    table.ForeignKey(
                        name: "FK_DutyModelEventModel_Duties_DutiesId",
                        column: x => x.DutiesId,
                        principalTable: "Duties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DutyModelEventModel_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DutyModelMilestoneModel",
                columns: table => new
                {
                    DutiesId = table.Column<long>(type: "INTEGER", nullable: false),
                    MilestonesId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DutyModelMilestoneModel", x => new { x.DutiesId, x.MilestonesId });
                    table.ForeignKey(
                        name: "FK_DutyModelMilestoneModel_Duties_DutiesId",
                        column: x => x.DutiesId,
                        principalTable: "Duties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DutyModelMilestoneModel_Milestones_MilestonesId",
                        column: x => x.MilestonesId,
                        principalTable: "Milestones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventModelMilestoneModel",
                columns: table => new
                {
                    EventsId = table.Column<long>(type: "INTEGER", nullable: false),
                    MilestonesId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventModelMilestoneModel", x => new { x.EventsId, x.MilestonesId });
                    table.ForeignKey(
                        name: "FK_EventModelMilestoneModel_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventModelMilestoneModel_Milestones_MilestonesId",
                        column: x => x.MilestonesId,
                        principalTable: "Milestones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_DosageUnitId",
                table: "Treatments",
                column: "DosageUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_DurationUnitId",
                table: "Treatments",
                column: "DurationUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_FrequencyUnitId",
                table: "Treatments",
                column: "FrequencyUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_RecipientMassUnitId",
                table: "Treatments",
                column: "RecipientMassUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentRecords_DosageUnitId",
                table: "TreatmentRecords",
                column: "DosageUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDuties_EventModelId",
                table: "ScheduledDuties",
                column: "EventModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Measures_UnitId",
                table: "Measures",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_MeasurementUnitId",
                table: "Measurements",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeeds_LivestockAnimalModelId",
                table: "LivestockFeeds",
                column: "LivestockAnimalModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Chores_FrequencyUnitId",
                table: "Chores",
                column: "FrequencyUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Chores_PeriodUnitId",
                table: "Chores",
                column: "PeriodUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ChoreModelDutyModel_DutiesId",
                table: "ChoreModelDutyModel",
                column: "DutiesId");

            migrationBuilder.CreateIndex(
                name: "IX_DutyModelEventModel_EventsId",
                table: "DutyModelEventModel",
                column: "EventsId");

            migrationBuilder.CreateIndex(
                name: "IX_DutyModelMilestoneModel_MilestonesId",
                table: "DutyModelMilestoneModel",
                column: "MilestonesId");

            migrationBuilder.CreateIndex(
                name: "IX_EventModelMilestoneModel_MilestonesId",
                table: "EventModelMilestoneModel",
                column: "MilestonesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chores_Units_FrequencyUnitId",
                table: "Chores",
                column: "FrequencyUnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chores_Units_PeriodUnitId",
                table: "Chores",
                column: "PeriodUnitId",
                principalTable: "Units",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LivestockFeeds_LivestockAnimals_LivestockAnimalModelId",
                table: "LivestockFeeds",
                column: "LivestockAnimalModelId",
                principalTable: "LivestockAnimals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Measurements_Units_MeasurementUnitId",
                table: "Measurements",
                column: "MeasurementUnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Measures_Units_UnitId",
                table: "Measures",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledDuties_Events_EventModelId",
                table: "ScheduledDuties",
                column: "EventModelId",
                principalTable: "Events",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentRecords_Units_DosageUnitId",
                table: "TreatmentRecords",
                column: "DosageUnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_Units_DosageUnitId",
                table: "Treatments",
                column: "DosageUnitId",
                principalTable: "Units",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_Units_DurationUnitId",
                table: "Treatments",
                column: "DurationUnitId",
                principalTable: "Units",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_Units_FrequencyUnitId",
                table: "Treatments",
                column: "FrequencyUnitId",
                principalTable: "Units",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_Units_RecipientMassUnitId",
                table: "Treatments",
                column: "RecipientMassUnitId",
                principalTable: "Units",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chores_Units_FrequencyUnitId",
                table: "Chores");

            migrationBuilder.DropForeignKey(
                name: "FK_Chores_Units_PeriodUnitId",
                table: "Chores");

            migrationBuilder.DropForeignKey(
                name: "FK_LivestockFeeds_LivestockAnimals_LivestockAnimalModelId",
                table: "LivestockFeeds");

            migrationBuilder.DropForeignKey(
                name: "FK_Measurements_Units_MeasurementUnitId",
                table: "Measurements");

            migrationBuilder.DropForeignKey(
                name: "FK_Measures_Units_UnitId",
                table: "Measures");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledDuties_Events_EventModelId",
                table: "ScheduledDuties");

            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentRecords_Units_DosageUnitId",
                table: "TreatmentRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_Units_DosageUnitId",
                table: "Treatments");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_Units_DurationUnitId",
                table: "Treatments");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_Units_FrequencyUnitId",
                table: "Treatments");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_Units_RecipientMassUnitId",
                table: "Treatments");

            migrationBuilder.DropTable(
                name: "ChoreModelDutyModel");

            migrationBuilder.DropTable(
                name: "DutyModelEventModel");

            migrationBuilder.DropTable(
                name: "DutyModelMilestoneModel");

            migrationBuilder.DropTable(
                name: "EventModelMilestoneModel");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_DosageUnitId",
                table: "Treatments");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_DurationUnitId",
                table: "Treatments");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_FrequencyUnitId",
                table: "Treatments");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_RecipientMassUnitId",
                table: "Treatments");

            migrationBuilder.DropIndex(
                name: "IX_TreatmentRecords_DosageUnitId",
                table: "TreatmentRecords");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledDuties_EventModelId",
                table: "ScheduledDuties");

            migrationBuilder.DropIndex(
                name: "IX_Measures_UnitId",
                table: "Measures");

            migrationBuilder.DropIndex(
                name: "IX_Measurements_MeasurementUnitId",
                table: "Measurements");

            migrationBuilder.DropIndex(
                name: "IX_LivestockFeeds_LivestockAnimalModelId",
                table: "LivestockFeeds");

            migrationBuilder.DropIndex(
                name: "IX_Chores_FrequencyUnitId",
                table: "Chores");

            migrationBuilder.DropIndex(
                name: "IX_Chores_PeriodUnitId",
                table: "Chores");

            migrationBuilder.DropColumn(
                name: "EventModelId",
                table: "ScheduledDuties");

            migrationBuilder.DropColumn(
                name: "LivestockAnimalModelId",
                table: "LivestockFeeds");

            migrationBuilder.AddColumn<long>(
                name: "RecipientId",
                table: "BreedingRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
