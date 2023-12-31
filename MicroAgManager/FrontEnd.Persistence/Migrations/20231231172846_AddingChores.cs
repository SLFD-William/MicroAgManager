using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddingChores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LandPlots_LandPlots_LandPlotModelId",
                table: "LandPlots");

            migrationBuilder.DropIndex(
                name: "IX_LandPlots_LandPlotModelId",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "LandPlotModelId",
                table: "LandPlots");

            migrationBuilder.AddColumn<long>(
                name: "ChoreModelId",
                table: "Duties",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Symbol = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    ConversionFactorToSIUnit = table.Column<double>(type: "REAL", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecipientTypeId = table.Column<long>(type: "INTEGER", nullable: false),
                    RecipientType = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Color = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    DueByTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    Singularly = table.Column<bool>(type: "INTEGER", nullable: false),
                    Frequency = table.Column<decimal>(type: "TEXT", precision: 18, scale: 3, nullable: false),
                    FrequencyUnitId = table.Column<long>(type: "INTEGER", nullable: false),
                    Period = table.Column<decimal>(type: "TEXT", precision: 18, scale: 3, nullable: true),
                    PeriodUnitId = table.Column<long>(type: "INTEGER", nullable: true),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chores_Unit_FrequencyUnitId",
                        column: x => x.FrequencyUnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chores_Unit_PeriodUnitId",
                        column: x => x.PeriodUnitId,
                        principalTable: "Unit",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LandPlots_FarmLocationId",
                table: "LandPlots",
                column: "FarmLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_LandPlots_ParentPlotId",
                table: "LandPlots",
                column: "ParentPlotId");

            migrationBuilder.CreateIndex(
                name: "IX_Duties_ChoreModelId",
                table: "Duties",
                column: "ChoreModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Chores_FrequencyUnitId",
                table: "Chores",
                column: "FrequencyUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Chores_PeriodUnitId",
                table: "Chores",
                column: "PeriodUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_ModifiedOn",
                table: "Unit",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_TenantId",
                table: "Unit",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Duties_Chores_ChoreModelId",
                table: "Duties",
                column: "ChoreModelId",
                principalTable: "Chores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LandPlots_Farms_FarmLocationId",
                table: "LandPlots",
                column: "FarmLocationId",
                principalTable: "Farms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LandPlots_LandPlots_ParentPlotId",
                table: "LandPlots",
                column: "ParentPlotId",
                principalTable: "LandPlots",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Duties_Chores_ChoreModelId",
                table: "Duties");

            migrationBuilder.DropForeignKey(
                name: "FK_LandPlots_Farms_FarmLocationId",
                table: "LandPlots");

            migrationBuilder.DropForeignKey(
                name: "FK_LandPlots_LandPlots_ParentPlotId",
                table: "LandPlots");

            migrationBuilder.DropTable(
                name: "Chores");

            migrationBuilder.DropTable(
                name: "Unit");

            migrationBuilder.DropIndex(
                name: "IX_LandPlots_FarmLocationId",
                table: "LandPlots");

            migrationBuilder.DropIndex(
                name: "IX_LandPlots_ParentPlotId",
                table: "LandPlots");

            migrationBuilder.DropIndex(
                name: "IX_Duties_ChoreModelId",
                table: "Duties");

            migrationBuilder.DropColumn(
                name: "ChoreModelId",
                table: "Duties");

            migrationBuilder.AddColumn<long>(
                name: "LandPlotModelId",
                table: "LandPlots",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandPlots_LandPlotModelId",
                table: "LandPlots",
                column: "LandPlotModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_LandPlots_LandPlots_LandPlotModelId",
                table: "LandPlots",
                column: "LandPlotModelId",
                principalTable: "LandPlots",
                principalColumn: "Id");
        }
    }
}
