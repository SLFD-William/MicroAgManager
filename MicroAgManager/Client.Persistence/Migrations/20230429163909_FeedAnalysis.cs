using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FeedAnalysis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LivestockFeedModelId",
                table: "LivestockFeedDistributions",
                type: "INTEGER",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeedDistributions_LivestockFeedModelId",
                table: "LivestockFeedDistributions",
                column: "LivestockFeedModelId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeedAnalysisResults_LivestockFeedAnalysisModelId",
                table: "LivestockFeedAnalysisResults",
                column: "LivestockFeedAnalysisModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_LivestockFeedDistributions_LivestockFeeds_LivestockFeedModelId",
                table: "LivestockFeedDistributions",
                column: "LivestockFeedModelId",
                principalTable: "LivestockFeeds",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LivestockFeedDistributions_LivestockFeeds_LivestockFeedModelId",
                table: "LivestockFeedDistributions");

            migrationBuilder.DropTable(
                name: "LivestockFeedAnalysisParameters");

            migrationBuilder.DropTable(
                name: "LivestockFeedAnalysisResults");

            migrationBuilder.DropTable(
                name: "LivestockFeedAnalyses");

            migrationBuilder.DropIndex(
                name: "IX_LivestockFeedDistributions_LivestockFeedModelId",
                table: "LivestockFeedDistributions");

            migrationBuilder.DropColumn(
                name: "LivestockFeedModelId",
                table: "LivestockFeedDistributions");
        }
    }
}
