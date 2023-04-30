using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    DaysDue = table.Column<int>(type: "INTEGER", nullable: false),
                    DutyType = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    DutyTypeId = table.Column<long>(type: "INTEGER", nullable: false),
                    Relationship = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Gender = table.Column<string>(type: "TEXT", maxLength: 1, nullable: true),
                    SystemRequired = table.Column<bool>(type: "INTEGER", nullable: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Color = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Milestones",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Subcategory = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    SystemRequired = table.Column<bool>(type: "INTEGER", nullable: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Milestones", x => x.Id);
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
                name: "ScheduledDuties",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DutyId = table.Column<long>(type: "INTEGER", nullable: false),
                    Dismissed = table.Column<bool>(type: "INTEGER", nullable: false),
                    DueOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReminderDays = table.Column<decimal>(type: "TEXT", precision: 18, scale: 3, nullable: false),
                    CompletedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CompletedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    DutyModelId = table.Column<long>(type: "INTEGER", nullable: true),
                    EventModelId = table.Column<long>(type: "INTEGER", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledDuties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledDuties_Duties_DutyModelId",
                        column: x => x.DutyModelId,
                        principalTable: "Duties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScheduledDuties_Events_EventModelId",
                        column: x => x.EventModelId,
                        principalTable: "Events",
                        principalColumn: "Id");
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

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDuties_CompletedOn",
                table: "ScheduledDuties",
                column: "CompletedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDuties_Dismissed",
                table: "ScheduledDuties",
                column: "Dismissed");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDuties_DueOn",
                table: "ScheduledDuties",
                column: "DueOn");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDuties_DutyModelId",
                table: "ScheduledDuties",
                column: "DutyModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDuties_EventModelId",
                table: "ScheduledDuties",
                column: "EventModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DutyModelEventModel");

            migrationBuilder.DropTable(
                name: "DutyModelMilestoneModel");

            migrationBuilder.DropTable(
                name: "EventModelMilestoneModel");

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
