﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceCodes",
                columns: table => new
                {
                    UserCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DeviceCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SubjectId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SessionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceCodes", x => x.UserCode);
                });

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
                name: "Keys",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Use = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Algorithm = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsX509Certificate = table.Column<bool>(type: "bit", nullable: false),
                    DataProtected = table.Column<bool>(type: "bit", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LivestockFeedAnalysisParameters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Parameter = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubParameter = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReportOrder = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_LivestockFeedAnalysisParameters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LivestockTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ParentMaleName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ParentFemaleName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    DefaultStatus = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Care = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
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
                    table.PrimaryKey("PK_LivestockTypes", x => x.Id);
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
                name: "PersistedGrants",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubjectId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SessionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConsumedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Data = table.Column<string>(type: "nvarchar(max)", maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedGrants", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    TenantUserAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccessLevel = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
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
                name: "LivestockBreeds",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LivestockId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    EmojiChar = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    GestationPeriod = table.Column<int>(type: "int", nullable: false),
                    HeatPeriod = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_LivestockBreeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivestockBreeds_LivestockTypes_LivestockId",
                        column: x => x.LivestockId,
                        principalTable: "LivestockTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LivestockFeeds",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LivestockTypeId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Source = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Cutting = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    QuantityUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    QuantityWarning = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    FeedType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Distribution = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
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
                    table.PrimaryKey("PK_LivestockFeeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivestockFeeds_LivestockTypes_LivestockTypeId",
                        column: x => x.LivestockTypeId,
                        principalTable: "LivestockTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LivestockStatuses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    LivestockTypeId = table.Column<long>(type: "bigint", nullable: false),
                    InMilk = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BeingManaged = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Sterile = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BottleFed = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ForSale = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
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
                    table.PrimaryKey("PK_LivestockStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivestockStatuses_LivestockTypes_LivestockTypeId",
                        column: x => x.LivestockTypeId,
                        principalTable: "LivestockTypes",
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

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Farms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Farms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Farms_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Livestocks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BreedId = table.Column<long>(type: "bigint", nullable: false),
                    MotherId = table.Column<long>(type: "bigint", nullable: true),
                    FatherId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Variety = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BeingManaged = table.Column<bool>(type: "bit", nullable: false),
                    BornDefective = table.Column<bool>(type: "bit", nullable: false),
                    BirthDefect = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Sterile = table.Column<bool>(type: "bit", nullable: false),
                    InMilk = table.Column<bool>(type: "bit", nullable: false),
                    BottleFed = table.Column<bool>(type: "bit", nullable: false),
                    ForSale = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_Livestocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Livestocks_LivestockBreeds_BreedId",
                        column: x => x.BreedId,
                        principalTable: "LivestockBreeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Livestocks_Livestocks_FatherId",
                        column: x => x.FatherId,
                        principalTable: "Livestocks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Livestocks_Livestocks_MotherId",
                        column: x => x.MotherId,
                        principalTable: "Livestocks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LivestockFeedAnalyses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabNumber = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    FeedId = table.Column<long>(type: "bigint", nullable: false),
                    TestCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    DateSampled = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateReceived = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateReported = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DatePrinted = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_LivestockFeedAnalyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivestockFeedAnalyses_LivestockFeeds_FeedId",
                        column: x => x.FeedId,
                        principalTable: "LivestockFeeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LivestockFeedDistributions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeedId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    Discarded = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DatePerformed = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_LivestockFeedDistributions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivestockFeedDistributions_LivestockFeeds_FeedId",
                        column: x => x.FeedId,
                        principalTable: "LivestockFeeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LivestockFeedServings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeedId = table.Column<long>(type: "bigint", nullable: false),
                    StatusId = table.Column<long>(type: "bigint", nullable: false),
                    ServingFrequency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Serving = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
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
                    table.PrimaryKey("PK_LivestockFeedServings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivestockFeedServings_LivestockFeeds_FeedId",
                        column: x => x.FeedId,
                        principalTable: "LivestockFeeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LivestockFeedServings_LivestockStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "LivestockStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction, onUpdate: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Plots",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Area = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    AreaUnit = table.Column<long>(type: "bigint", nullable: false),
                    Usage = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FarmLocationId = table.Column<long>(type: "bigint", nullable: false),
                    ParentPlotId = table.Column<long>(type: "bigint", nullable: true),
                    LandPlotId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Plots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plots_Farms_FarmLocationId",
                        column: x => x.FarmLocationId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Plots_Plots_LandPlotId",
                        column: x => x.LandPlotId,
                        principalTable: "Plots",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LivestockLivestockStatus",
                columns: table => new
                {
                    LivestocksId = table.Column<long>(type: "bigint", nullable: false),
                    StatusesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockLivestockStatus", x => new { x.LivestocksId, x.StatusesId });
                    table.ForeignKey(
                        name: "FK_LivestockLivestockStatus_LivestockStatuses_StatusesId",
                        column: x => x.StatusesId,
                        principalTable: "LivestockStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LivestockLivestockStatus_Livestocks_LivestocksId",
                        column: x => x.LivestocksId,
                        principalTable: "Livestocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction,onUpdate:ReferentialAction.NoAction);
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
                name: "LivestockFeedAnalysisResults",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnalysisId = table.Column<long>(type: "bigint", nullable: false),
                    ParameterId = table.Column<long>(type: "bigint", nullable: false),
                    AsFed = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Dry = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
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
                    table.PrimaryKey("PK_LivestockFeedAnalysisResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivestockFeedAnalysisResults_LivestockFeedAnalyses_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "LivestockFeedAnalyses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LivestockFeedAnalysisResults_LivestockFeedAnalysisParameters_ParameterId",
                        column: x => x.ParameterId,
                        principalTable: "LivestockFeedAnalysisParameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LandPlotLivestock",
                columns: table => new
                {
                    LivestocksId = table.Column<long>(type: "bigint", nullable: false),
                    LocationsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandPlotLivestock", x => new { x.LivestocksId, x.LocationsId });
                    table.ForeignKey(
                        name: "FK_LandPlotLivestock_Livestocks_LivestocksId",
                        column: x => x.LivestocksId,
                        principalTable: "Livestocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LandPlotLivestock_Plots_LocationsId",
                        column: x => x.LocationsId,
                        principalTable: "Plots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "cd10345b-bc5d-4ce6-0e98-08d76b81d46d", "1", "SystemAdmin", "SystemAdmin" },
                    { "cd10345b-bc5d-4ce6-0e98-08d76b81d46e", "1", "TenantAdmin", "TenantAdmin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_ApplicationUserId",
                table: "AspNetUserClaims",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_ApplicationUserId",
                table: "AspNetUserLogins",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_ApplicationUserId",
                table: "AspNetUserRoles",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TenantId",
                table: "AspNetUsers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserTokens_ApplicationUserId",
                table: "AspNetUserTokens",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCodes_DeviceCode",
                table: "DeviceCodes",
                column: "DeviceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCodes_Expiration",
                table: "DeviceCodes",
                column: "Expiration");

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
                name: "Index_FarmLocation_Deleted",
                table: "Farms",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "Index_FarmLocation_ModifiedOn",
                table: "Farms",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "Index_FarmLocation_TenantIdAndPrimaryKey",
                table: "Farms",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Farms_TenantId",
                table: "Farms",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Keys_Use",
                table: "Keys",
                column: "Use");

            migrationBuilder.CreateIndex(
                name: "IX_LandPlotLivestock_LocationsId",
                table: "LandPlotLivestock",
                column: "LocationsId");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockBreed_Deleted",
                table: "LivestockBreeds",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockBreed_ModifiedOn",
                table: "LivestockBreeds",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockBreed_TenantIdAndPrimaryKey",
                table: "LivestockBreeds",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_LivestockBreeds_LivestockId",
                table: "LivestockBreeds",
                column: "LivestockId");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockFeedAnalysis_Deleted",
                table: "LivestockFeedAnalyses",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockFeedAnalysis_ModifiedOn",
                table: "LivestockFeedAnalyses",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockFeedAnalysis_TenantIdAndPrimaryKey",
                table: "LivestockFeedAnalyses",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeedAnalyses_FeedId",
                table: "LivestockFeedAnalyses",
                column: "FeedId");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockFeedAnalysisParameter_Deleted",
                table: "LivestockFeedAnalysisParameters",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockFeedAnalysisParameter_ModifiedOn",
                table: "LivestockFeedAnalysisParameters",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockFeedAnalysisParameter_TenantIdAndPrimaryKey",
                table: "LivestockFeedAnalysisParameters",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "Index_LivestockFeedAnalysisResult_Deleted",
                table: "LivestockFeedAnalysisResults",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockFeedAnalysisResult_ModifiedOn",
                table: "LivestockFeedAnalysisResults",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockFeedAnalysisResult_TenantIdAndPrimaryKey",
                table: "LivestockFeedAnalysisResults",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeedAnalysisResults_AnalysisId",
                table: "LivestockFeedAnalysisResults",
                column: "AnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeedAnalysisResults_ParameterId",
                table: "LivestockFeedAnalysisResults",
                column: "ParameterId");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockFeedDistribution_Deleted",
                table: "LivestockFeedDistributions",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockFeedDistribution_ModifiedOn",
                table: "LivestockFeedDistributions",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockFeedDistribution_TenantIdAndPrimaryKey",
                table: "LivestockFeedDistributions",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeedDistributions_FeedId",
                table: "LivestockFeedDistributions",
                column: "FeedId");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockFeed_Deleted",
                table: "LivestockFeeds",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockFeed_ModifiedOn",
                table: "LivestockFeeds",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockFeed_TenantIdAndPrimaryKey",
                table: "LivestockFeeds",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeeds_LivestockTypeId",
                table: "LivestockFeeds",
                column: "LivestockTypeId");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockFeedServing_Deleted",
                table: "LivestockFeedServings",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockFeedServing_ModifiedOn",
                table: "LivestockFeedServings",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockFeedServing_TenantIdAndPrimaryKey",
                table: "LivestockFeedServings",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeedServings_FeedId",
                table: "LivestockFeedServings",
                column: "FeedId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeedServings_StatusId",
                table: "LivestockFeedServings",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockLivestockStatus_StatusesId",
                table: "LivestockLivestockStatus",
                column: "StatusesId");

            migrationBuilder.CreateIndex(
                name: "Index_Animal_BeingManaged",
                table: "Livestocks",
                column: "BeingManaged");

            migrationBuilder.CreateIndex(
                name: "Index_Birthday",
                table: "Livestocks",
                column: "Birthdate");

            migrationBuilder.CreateIndex(
                name: "Index_Livestock_Deleted",
                table: "Livestocks",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "Index_Livestock_ModifiedOn",
                table: "Livestocks",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "Index_Livestock_TenantIdAndPrimaryKey",
                table: "Livestocks",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "Index_Name",
                table: "Livestocks",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Livestocks_BreedId",
                table: "Livestocks",
                column: "BreedId");

            migrationBuilder.CreateIndex(
                name: "IX_Livestocks_FatherId",
                table: "Livestocks",
                column: "FatherId",
                unique: true,
                filter: "[FatherId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Livestocks_MotherId",
                table: "Livestocks",
                column: "MotherId",
                unique: true,
                filter: "[MotherId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockStatus_Deleted",
                table: "LivestockStatuses",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockStatus_ModifiedOn",
                table: "LivestockStatuses",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockStatus_TenantIdAndPrimaryKey",
                table: "LivestockStatuses",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_LivestockStatuses_LivestockTypeId",
                table: "LivestockStatuses",
                column: "LivestockTypeId");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockType_Deleted",
                table: "LivestockTypes",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockType_ModifiedOn",
                table: "LivestockTypes",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockType_TenantIdAndPrimaryKey",
                table: "LivestockTypes",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_LivestockTypes_Name",
                table: "LivestockTypes",
                column: "Name",
                unique: true);

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
                name: "IX_PersistedGrants_ConsumedTime",
                table: "PersistedGrants",
                column: "ConsumedTime");

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_Expiration",
                table: "PersistedGrants",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_SubjectId_ClientId_Type",
                table: "PersistedGrants",
                columns: new[] { "SubjectId", "ClientId", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_SubjectId_SessionId_Type",
                table: "PersistedGrants",
                columns: new[] { "SubjectId", "SessionId", "Type" });

            migrationBuilder.CreateIndex(
                name: "Index_LandPlot_Deleted",
                table: "Plots",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "Index_LandPlot_ModifiedOn",
                table: "Plots",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "Index_LandPlot_TenantIdAndPrimaryKey",
                table: "Plots",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Plots_FarmLocationId",
                table: "Plots",
                column: "FarmLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Plots_LandPlotId",
                table: "Plots",
                column: "LandPlotId");

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
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DeviceCodes");

            migrationBuilder.DropTable(
                name: "DutyEvent");

            migrationBuilder.DropTable(
                name: "DutyMilestone");

            migrationBuilder.DropTable(
                name: "EventMilestone");

            migrationBuilder.DropTable(
                name: "Keys");

            migrationBuilder.DropTable(
                name: "LandPlotLivestock");

            migrationBuilder.DropTable(
                name: "LivestockFeedAnalysisResults");

            migrationBuilder.DropTable(
                name: "LivestockFeedDistributions");

            migrationBuilder.DropTable(
                name: "LivestockFeedServings");

            migrationBuilder.DropTable(
                name: "LivestockLivestockStatus");

            migrationBuilder.DropTable(
                name: "PersistedGrants");

            migrationBuilder.DropTable(
                name: "ScheduledDuties");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Milestones");

            migrationBuilder.DropTable(
                name: "Plots");

            migrationBuilder.DropTable(
                name: "LivestockFeedAnalyses");

            migrationBuilder.DropTable(
                name: "LivestockFeedAnalysisParameters");

            migrationBuilder.DropTable(
                name: "LivestockStatuses");

            migrationBuilder.DropTable(
                name: "Duties");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Livestocks");

            migrationBuilder.DropTable(
                name: "Farms");

            migrationBuilder.DropTable(
                name: "LivestockFeeds");

            migrationBuilder.DropTable(
                name: "LivestockBreeds");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "LivestockTypes");
        }
    }
}