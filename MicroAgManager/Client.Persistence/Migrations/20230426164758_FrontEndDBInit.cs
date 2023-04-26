using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FrontEndDBInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlotUseEnum",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotUseEnum", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
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
                name: "UnitEnum",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitEnum", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Farms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Longitude = table.Column<string>(type: "TEXT", nullable: true),
                    Latitude = table.Column<string>(type: "TEXT", nullable: true),
                    StreetAddress = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    State = table.Column<string>(type: "TEXT", nullable: false),
                    Zip = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: true),
                    TenantModelId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Farms_Tenants_TenantModelId",
                        column: x => x.TenantModelId,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LandPlotModel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FarmLocationId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Area = table.Column<decimal>(type: "TEXT", nullable: false),
                    AreaUnitId = table.Column<long>(type: "INTEGER", nullable: false),
                    UsageId = table.Column<long>(type: "INTEGER", nullable: false),
                    ParentPlotId = table.Column<long>(type: "INTEGER", nullable: true),
                    FarmLocationModelId = table.Column<long>(type: "INTEGER", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandPlotModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LandPlotModel_Farms_FarmLocationModelId",
                        column: x => x.FarmLocationModelId,
                        principalTable: "Farms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LandPlotModel_PlotUseEnum_UsageId",
                        column: x => x.UsageId,
                        principalTable: "PlotUseEnum",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LandPlotModel_UnitEnum_AreaUnitId",
                        column: x => x.AreaUnitId,
                        principalTable: "UnitEnum",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Farms_TenantModelId",
                table: "Farms",
                column: "TenantModelId");

            migrationBuilder.CreateIndex(
                name: "IX_LandPlotModel_AreaUnitId",
                table: "LandPlotModel",
                column: "AreaUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_LandPlotModel_FarmLocationModelId",
                table: "LandPlotModel",
                column: "FarmLocationModelId");

            migrationBuilder.CreateIndex(
                name: "IX_LandPlotModel_UsageId",
                table: "LandPlotModel",
                column: "UsageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LandPlotModel");

            migrationBuilder.DropTable(
                name: "Farms");

            migrationBuilder.DropTable(
                name: "PlotUseEnum");

            migrationBuilder.DropTable(
                name: "UnitEnum");

            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
