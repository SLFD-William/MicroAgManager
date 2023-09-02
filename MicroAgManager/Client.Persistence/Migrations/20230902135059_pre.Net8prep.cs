using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class preNet8prep : Migration
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
                    LivestockAnimalId = table.Column<long>(type: "INTEGER", nullable: true),
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
                name: "Farms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: true),
                    Latitude = table.Column<double>(type: "REAL", nullable: true),
                    StreetAddress = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    State = table.Column<string>(type: "TEXT", nullable: false),
                    Zip = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: true),
                    CountryCode = table.Column<string>(type: "TEXT", maxLength: 2, nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandPlots",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FarmLocationId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Area = table.Column<decimal>(type: "TEXT", precision: 18, scale: 3, nullable: false),
                    AreaUnit = table.Column<string>(type: "TEXT", nullable: false),
                    Usage = table.Column<string>(type: "TEXT", nullable: false),
                    ParentPlotId = table.Column<long>(type: "INTEGER", nullable: true),
                    LandPlotModelId = table.Column<long>(type: "INTEGER", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandPlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LandPlots_LandPlots_LandPlotModelId",
                        column: x => x.LandPlotModelId,
                        principalTable: "LandPlots",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LivestockFeedAnalyses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LabNumber = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    FeedId = table.Column<long>(type: "INTEGER", nullable: false),
                    TestCode = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    DateSampled = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateReceived = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateReported = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DatePrinted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockFeedAnalyses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LivestockFeedAnalysisParameters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Parameter = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SubParameter = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Unit = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Method = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ReportOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockFeedAnalysisParameters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LivestockAnimals",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    GroupName = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    ParentMaleName = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    ParentFemaleName = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Care = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockAnimals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Level = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    EventName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Milestones",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Subcategory = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    SystemRequired = table.Column<bool>(type: "INTEGER", nullable: false),
                    LivestockAnimalId = table.Column<long>(type: "INTEGER", nullable: true),
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
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GuidId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    TenantUserAdminId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
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
                name: "LivestockFeedAnalysisResults",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AnalysisId = table.Column<long>(type: "INTEGER", nullable: false),
                    ParameterId = table.Column<long>(type: "INTEGER", nullable: false),
                    AsFed = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Dry = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    LivestockFeedAnalysisModelId = table.Column<long>(type: "INTEGER", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockFeedAnalysisResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivestockFeedAnalysisResults_LivestockFeedAnalyses_LivestockFeedAnalysisModelId",
                        column: x => x.LivestockFeedAnalysisModelId,
                        principalTable: "LivestockFeedAnalyses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LivestockBreeds",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LivestockAnimalId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    EmojiChar = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    GestationPeriod = table.Column<int>(type: "INTEGER", nullable: false),
                    HeatPeriod = table.Column<int>(type: "INTEGER", nullable: false),
                    LivestockAnimalModelId = table.Column<long>(type: "INTEGER", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockBreeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivestockBreeds_LivestockAnimals_LivestockAnimalModelId",
                        column: x => x.LivestockAnimalModelId,
                        principalTable: "LivestockAnimals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LivestockFeeds",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LivestockAnimalId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Source = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Cutting = table.Column<int>(type: "INTEGER", nullable: true),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", precision: 18, scale: 3, nullable: false),
                    QuantityUnit = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    QuantityWarning = table.Column<decimal>(type: "TEXT", precision: 18, scale: 3, nullable: false),
                    FeedType = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Distribution = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    LivestockAnimalModelId = table.Column<long>(type: "INTEGER", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockFeeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivestockFeeds_LivestockAnimals_LivestockAnimalModelId",
                        column: x => x.LivestockAnimalModelId,
                        principalTable: "LivestockAnimals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LivestockStatuses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LivestockAnimalId = table.Column<long>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    DefaultStatus = table.Column<bool>(type: "INTEGER", nullable: false),
                    BeingManaged = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Sterile = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    InMilk = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    BottleFed = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    ForSale = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    LivestockAnimalModelId = table.Column<long>(type: "INTEGER", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivestockStatuses_LivestockAnimals_LivestockAnimalModelId",
                        column: x => x.LivestockAnimalModelId,
                        principalTable: "LivestockAnimals",
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

            migrationBuilder.CreateTable(
                name: "Livestocks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MotherId = table.Column<long>(type: "INTEGER", nullable: true),
                    FatherId = table.Column<long>(type: "INTEGER", nullable: true),
                    LivestockBreedId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Birthdate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Gender = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Variety = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    BeingManaged = table.Column<bool>(type: "INTEGER", nullable: false),
                    BornDefective = table.Column<bool>(type: "INTEGER", nullable: false),
                    BirthDefect = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Sterile = table.Column<bool>(type: "INTEGER", nullable: false),
                    InMilk = table.Column<bool>(type: "INTEGER", nullable: false),
                    BottleFed = table.Column<bool>(type: "INTEGER", nullable: false),
                    ForSale = table.Column<bool>(type: "INTEGER", nullable: false),
                    LivestockBreedModelId = table.Column<long>(type: "INTEGER", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livestocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Livestocks_LivestockBreeds_LivestockBreedModelId",
                        column: x => x.LivestockBreedModelId,
                        principalTable: "LivestockBreeds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LivestockFeedDistributions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FeedId = table.Column<long>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", precision: 18, scale: 3, nullable: false),
                    Discarded = table.Column<bool>(type: "INTEGER", nullable: false),
                    Note = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DatePerformed = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LivestockFeedModelId = table.Column<long>(type: "INTEGER", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockFeedDistributions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivestockFeedDistributions_LivestockFeeds_LivestockFeedModelId",
                        column: x => x.LivestockFeedModelId,
                        principalTable: "LivestockFeeds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LivestockFeedServings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FeedId = table.Column<long>(type: "INTEGER", nullable: false),
                    StatusId = table.Column<long>(type: "INTEGER", nullable: false),
                    ServingFrequency = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Serving = table.Column<decimal>(type: "TEXT", precision: 18, scale: 3, nullable: false),
                    LivestockFeedModelId = table.Column<long>(type: "INTEGER", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockFeedServings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivestockFeedServings_LivestockFeeds_LivestockFeedModelId",
                        column: x => x.LivestockFeedModelId,
                        principalTable: "LivestockFeeds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LandPlotModelLivestockModel",
                columns: table => new
                {
                    LivestocksId = table.Column<long>(type: "INTEGER", nullable: false),
                    LocationsId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandPlotModelLivestockModel", x => new { x.LivestocksId, x.LocationsId });
                    table.ForeignKey(
                        name: "FK_LandPlotModelLivestockModel_LandPlots_LocationsId",
                        column: x => x.LocationsId,
                        principalTable: "LandPlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LandPlotModelLivestockModel_Livestocks_LivestocksId",
                        column: x => x.LivestocksId,
                        principalTable: "Livestocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LivestockModelLivestockStatusModel",
                columns: table => new
                {
                    LivestocksId = table.Column<long>(type: "INTEGER", nullable: false),
                    StatusesId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockModelLivestockStatusModel", x => new { x.LivestocksId, x.StatusesId });
                    table.ForeignKey(
                        name: "FK_LivestockModelLivestockStatusModel_LivestockStatuses_StatusesId",
                        column: x => x.StatusesId,
                        principalTable: "LivestockStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LivestockModelLivestockStatusModel_Livestocks_LivestocksId",
                        column: x => x.LivestocksId,
                        principalTable: "Livestocks",
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
                name: "IX_LandPlotModelLivestockModel_LocationsId",
                table: "LandPlotModelLivestockModel",
                column: "LocationsId");

            migrationBuilder.CreateIndex(
                name: "IX_LandPlots_LandPlotModelId",
                table: "LandPlots",
                column: "LandPlotModelId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockBreeds_LivestockAnimalModelId",
                table: "LivestockBreeds",
                column: "LivestockAnimalModelId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeedAnalysisResults_LivestockFeedAnalysisModelId",
                table: "LivestockFeedAnalysisResults",
                column: "LivestockFeedAnalysisModelId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeedDistributions_LivestockFeedModelId",
                table: "LivestockFeedDistributions",
                column: "LivestockFeedModelId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeeds_LivestockAnimalModelId",
                table: "LivestockFeeds",
                column: "LivestockAnimalModelId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeedServings_LivestockFeedModelId",
                table: "LivestockFeedServings",
                column: "LivestockFeedModelId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockModelLivestockStatusModel_StatusesId",
                table: "LivestockModelLivestockStatusModel",
                column: "StatusesId");

            migrationBuilder.CreateIndex(
                name: "IX_Livestocks_LivestockBreedModelId",
                table: "Livestocks",
                column: "LivestockBreedModelId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockStatuses_LivestockAnimalModelId",
                table: "LivestockStatuses",
                column: "LivestockAnimalModelId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockAnimals_Name",
                table: "LivestockAnimals",
                column: "Name",
                unique: true);

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
                name: "Farms");

            migrationBuilder.DropTable(
                name: "LandPlotModelLivestockModel");

            migrationBuilder.DropTable(
                name: "LivestockFeedAnalysisParameters");

            migrationBuilder.DropTable(
                name: "LivestockFeedAnalysisResults");

            migrationBuilder.DropTable(
                name: "LivestockFeedDistributions");

            migrationBuilder.DropTable(
                name: "LivestockFeedServings");

            migrationBuilder.DropTable(
                name: "LivestockModelLivestockStatusModel");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "ScheduledDuties");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "Milestones");

            migrationBuilder.DropTable(
                name: "LandPlots");

            migrationBuilder.DropTable(
                name: "LivestockFeedAnalyses");

            migrationBuilder.DropTable(
                name: "LivestockFeeds");

            migrationBuilder.DropTable(
                name: "LivestockStatuses");

            migrationBuilder.DropTable(
                name: "Livestocks");

            migrationBuilder.DropTable(
                name: "Duties");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "LivestockBreeds");

            migrationBuilder.DropTable(
                name: "LivestockAnimals");
        }
    }
}
