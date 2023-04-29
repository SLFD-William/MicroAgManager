using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class LivestockFeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Area",
                table: "Plots",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,3)",
                oldPrecision: 20,
                oldScale: 3);

            migrationBuilder.AddColumn<string>(
                name: "Distribution",
                table: "LivestockFeeds",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

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
                        onDelete: ReferentialAction.NoAction,
                        onUpdate:ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LivestockFeedServings_LivestockStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "LivestockStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction,
                        onUpdate: ReferentialAction.NoAction);
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LivestockFeedDistributions");

            migrationBuilder.DropTable(
                name: "LivestockFeedServings");

            migrationBuilder.DropColumn(
                name: "Distribution",
                table: "LivestockFeeds");

            migrationBuilder.AlterColumn<decimal>(
                name: "Area",
                table: "Plots",
                type: "decimal(20,3)",
                precision: 20,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)",
                oldPrecision: 18,
                oldScale: 3);
        }
    }
}
