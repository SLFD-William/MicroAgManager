using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InterfaceFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EventId",
                table: "ScheduledDuties",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuantityUnit",
                table: "LivestockFeeds",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ChoreDuty",
                columns: table => new
                {
                    ChoresId = table.Column<long>(type: "bigint", nullable: false),
                    DutiesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChoreDuty", x => new { x.ChoresId, x.DutiesId });
                    table.ForeignKey(
                        name: "FK_ChoreDuty_Chores_ChoresId",
                        column: x => x.ChoresId,
                        principalTable: "Chores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChoreDuty_Duties_DutiesId",
                        column: x => x.DutiesId,
                        principalTable: "Duties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DutyEvent",
                columns: table => new
                {
                    DutiesId = table.Column<long>(type: "bigint", nullable: false),
                    EventsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DutyEvent", x => new { x.DutiesId, x.EventsId });
                    table.ForeignKey(
                        name: "FK_DutyEvent_Duties_DutiesId",
                        column: x => x.DutiesId,
                        principalTable: "Duties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DutyEvent_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DutyMilestone",
                columns: table => new
                {
                    DutiesId = table.Column<long>(type: "bigint", nullable: false),
                    MilestonesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DutyMilestone", x => new { x.DutiesId, x.MilestonesId });
                    table.ForeignKey(
                        name: "FK_DutyMilestone_Duties_DutiesId",
                        column: x => x.DutiesId,
                        principalTable: "Duties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DutyMilestone_Milestones_MilestonesId",
                        column: x => x.MilestonesId,
                        principalTable: "Milestones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventMilestone",
                columns: table => new
                {
                    EventsId = table.Column<long>(type: "bigint", nullable: false),
                    MilestonesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventMilestone", x => new { x.EventsId, x.MilestonesId });
                    table.ForeignKey(
                        name: "FK_EventMilestone_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventMilestone_Milestones_MilestonesId",
                        column: x => x.MilestonesId,
                        principalTable: "Milestones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDuties_EventId",
                table: "ScheduledDuties",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_MeasurementUnitId",
                table: "Measurements",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Chores_FrequencyUnitId",
                table: "Chores",
                column: "FrequencyUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Chores_PeriodUnitId",
                table: "Chores",
                column: "PeriodUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ChoreDuty_DutiesId",
                table: "ChoreDuty",
                column: "DutiesId");

            migrationBuilder.CreateIndex(
                name: "IX_DutyEvent_EventsId",
                table: "DutyEvent",
                column: "EventsId");

            migrationBuilder.CreateIndex(
                name: "IX_DutyMilestone_MilestonesId",
                table: "DutyMilestone",
                column: "MilestonesId");

            migrationBuilder.CreateIndex(
                name: "IX_EventMilestone_MilestonesId",
                table: "EventMilestone",
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
                name: "FK_Measurements_Units_MeasurementUnitId",
                table: "Measurements",
                column: "MeasurementUnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledDuties_Events_EventId",
                table: "ScheduledDuties",
                column: "EventId",
                principalTable: "Events",
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
                name: "FK_Measurements_Units_MeasurementUnitId",
                table: "Measurements");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledDuties_Events_EventId",
                table: "ScheduledDuties");

            migrationBuilder.DropTable(
                name: "ChoreDuty");

            migrationBuilder.DropTable(
                name: "DutyEvent");

            migrationBuilder.DropTable(
                name: "DutyMilestone");

            migrationBuilder.DropTable(
                name: "EventMilestone");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledDuties_EventId",
                table: "ScheduledDuties");

            migrationBuilder.DropIndex(
                name: "IX_Measurements_MeasurementUnitId",
                table: "Measurements");

            migrationBuilder.DropIndex(
                name: "IX_Chores_FrequencyUnitId",
                table: "Chores");

            migrationBuilder.DropIndex(
                name: "IX_Chores_PeriodUnitId",
                table: "Chores");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "ScheduledDuties");

            migrationBuilder.DropColumn(
                name: "QuantityUnit",
                table: "LivestockFeeds");
        }
    }
}
