using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FeedAnalysis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LivestockFeedAnalysisResults");

            migrationBuilder.DropTable(
                name: "LivestockFeedAnalyses");

            migrationBuilder.DropTable(
                name: "LivestockFeedAnalysisParameters");
        }
    }
}
