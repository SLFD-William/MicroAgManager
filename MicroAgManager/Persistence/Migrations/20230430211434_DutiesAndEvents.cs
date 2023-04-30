using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DutiesAndEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Duties",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    DaysDue = table.Column<int>(type: "int", nullable: false),
                    DutyType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DutyTypeId = table.Column<long>(type: "bigint", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    Relationship = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SystemRequired = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Milestones",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subcategory = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    SystemRequired = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Milestones", x => x.Id);
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
                        onDelete: ReferentialAction.NoAction, onUpdate:ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_DutyEvent_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction, onUpdate: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledDuties",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DutyId = table.Column<long>(type: "bigint", nullable: false),
                    Dismissed = table.Column<bool>(type: "bit", nullable: false),
                    DueOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReminderDays = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    CompletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EventId = table.Column<long>(type: "bigint", nullable: true),
                    LivestockId = table.Column<long>(type: "bigint", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledDuties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledDuties_Duties_DutyId",
                        column: x => x.DutyId,
                        principalTable: "Duties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduledDuties_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScheduledDuties_Livestocks_LivestockId",
                        column: x => x.LivestockId,
                        principalTable: "Livestocks",
                        principalColumn: "Id");
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
                name: "Index_Duty_Deleted",
                table: "Duties",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "Index_Duty_ModifiedOn",
                table: "Duties",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "Index_Duty_TenantIdAndPrimaryKey",
                table: "Duties",
                columns: new[] { "Id", "TenantId" });

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

            migrationBuilder.CreateIndex(
                name: "Index_Event_Deleted",
                table: "Events",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "Index_Event_ModifiedOn",
                table: "Events",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "Index_Event_TenantIdAndPrimaryKey",
                table: "Events",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "Index_Milestone_Deleted",
                table: "Milestones",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "Index_Milestone_ModifiedOn",
                table: "Milestones",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "Index_Milestone_TenantIdAndPrimaryKey",
                table: "Milestones",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "Index_ScheduledDuty_CompletedOn",
                table: "ScheduledDuties",
                column: "CompletedOn");

            migrationBuilder.CreateIndex(
                name: "Index_ScheduledDuty_Deleted",
                table: "ScheduledDuties",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "Index_ScheduledDuty_Dismissed",
                table: "ScheduledDuties",
                column: "Dismissed");

            migrationBuilder.CreateIndex(
                name: "Index_ScheduledDuty_DueOn",
                table: "ScheduledDuties",
                column: "DueOn");

            migrationBuilder.CreateIndex(
                name: "Index_ScheduledDuty_ModifiedOn",
                table: "ScheduledDuties",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "Index_ScheduledDuty_TenantIdAndPrimaryKey",
                table: "ScheduledDuties",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDuties_DutyId",
                table: "ScheduledDuties",
                column: "DutyId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDuties_EventId",
                table: "ScheduledDuties",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDuties_LivestockId",
                table: "ScheduledDuties",
                column: "LivestockId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DutyEvent");

            migrationBuilder.DropTable(
                name: "DutyMilestone");

            migrationBuilder.DropTable(
                name: "EventMilestone");

            migrationBuilder.DropTable(
                name: "ScheduledDuties");

            migrationBuilder.DropTable(
                name: "Milestones");

            migrationBuilder.DropTable(
                name: "Duties");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
