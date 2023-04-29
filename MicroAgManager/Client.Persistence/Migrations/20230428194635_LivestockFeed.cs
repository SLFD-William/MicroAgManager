using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class LivestockFeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Usage",
                table: "LandPlots",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER");

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
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockFeedDistributions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LivestockTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    GroupName = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    ParentMaleName = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    ParentFemaleName = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    DefaultStatus = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Care = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LivestockBreeds",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LivestockTypeId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    EmojiChar = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    GestationPeriod = table.Column<int>(type: "INTEGER", nullable: false),
                    HeatPeriod = table.Column<int>(type: "INTEGER", nullable: false),
                    LivestockTypeModelId = table.Column<long>(type: "INTEGER", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockBreeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivestockBreeds_LivestockTypes_LivestockTypeModelId",
                        column: x => x.LivestockTypeModelId,
                        principalTable: "LivestockTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LivestockFeeds",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LivestockTypeId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Source = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Cutting = table.Column<int>(type: "INTEGER", nullable: true),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", precision: 18, scale: 3, nullable: false),
                    QuantityUnit = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    QuantityWarning = table.Column<decimal>(type: "TEXT", precision: 18, scale: 3, nullable: false),
                    FeedType = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Distribution = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    LivestockTypeModelId = table.Column<long>(type: "INTEGER", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockFeeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivestockFeeds_LivestockTypes_LivestockTypeModelId",
                        column: x => x.LivestockTypeModelId,
                        principalTable: "LivestockTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LivestockStatuses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LivestockTypeId = table.Column<long>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    BeingManaged = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Sterile = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    InMilk = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    BottleFed = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    ForSale = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    LivestockTypeModelId = table.Column<long>(type: "INTEGER", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivestockStatuses_LivestockTypes_LivestockTypeModelId",
                        column: x => x.LivestockTypeModelId,
                        principalTable: "LivestockTypes",
                        principalColumn: "Id");
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
                        onDelete: ReferentialAction.NoAction, onUpdate: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LivestockModelLivestockStatusModel_Livestocks_LivestocksId",
                        column: x => x.LivestocksId,
                        principalTable: "Livestocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction, onUpdate: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LivestockBreeds_LivestockTypeModelId",
                table: "LivestockBreeds",
                column: "LivestockTypeModelId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeeds_LivestockTypeModelId",
                table: "LivestockFeeds",
                column: "LivestockTypeModelId");

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
                name: "IX_LivestockStatuses_LivestockTypeModelId",
                table: "LivestockStatuses",
                column: "LivestockTypeModelId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockTypes_Name",
                table: "LivestockTypes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LivestockFeedDistributions");

            migrationBuilder.DropTable(
                name: "LivestockFeedServings");

            migrationBuilder.DropTable(
                name: "LivestockModelLivestockStatusModel");

            migrationBuilder.DropTable(
                name: "LivestockFeeds");

            migrationBuilder.DropTable(
                name: "LivestockStatuses");

            migrationBuilder.DropTable(
                name: "Livestocks");

            migrationBuilder.DropTable(
                name: "LivestockBreeds");

            migrationBuilder.DropTable(
                name: "LivestockTypes");

            migrationBuilder.AlterColumn<long>(
                name: "Usage",
                table: "LandPlots",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
