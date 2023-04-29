using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Livestock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LivestockType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ParentMaleName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ParentFemaleName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    DefaultStatus = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Care = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_LivestockType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LivestockBreed",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LivestockId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmojiChar = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_LivestockBreed", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivestockBreed_LivestockType_LivestockId",
                        column: x => x.LivestockId,
                        principalTable: "LivestockType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LivestockFeed",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LivestockTypeId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_LivestockFeed", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivestockFeed_LivestockType_LivestockTypeId",
                        column: x => x.LivestockTypeId,
                        principalTable: "LivestockType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LivestockStatus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    LivestockTypeId = table.Column<long>(type: "bigint", nullable: false),
                    InMilk = table.Column<long>(type: "bigint", nullable: false),
                    BeingManaged = table.Column<long>(type: "bigint", nullable: false),
                    Sterile = table.Column<long>(type: "bigint", nullable: false),
                    BottleFed = table.Column<long>(type: "bigint", nullable: false),
                    ForSale = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_LivestockStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivestockStatus_LivestockType_LivestockTypeId",
                        column: x => x.LivestockTypeId,
                        principalTable: "LivestockType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Livestock",
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
                    table.PrimaryKey("PK_Livestock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Livestock_LivestockBreed_BreedId",
                        column: x => x.BreedId,
                        principalTable: "LivestockBreed",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Livestock_Livestock_FatherId",
                        column: x => x.FatherId,
                        principalTable: "Livestock",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Livestock_Livestock_MotherId",
                        column: x => x.MotherId,
                        principalTable: "Livestock",
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
                        name: "FK_LivestockLivestockStatus_LivestockStatus_StatusesId",
                        column: x => x.StatusesId,
                        principalTable: "LivestockStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction,
                        onUpdate: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LivestockLivestockStatus_Livestock_LivestocksId",
                        column: x => x.LivestocksId,
                        principalTable: "Livestock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction,
                        onUpdate:ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "Index_Animal_BeingManaged",
                table: "Livestock",
                column: "BeingManaged");

            migrationBuilder.CreateIndex(
                name: "Index_Birthday",
                table: "Livestock",
                column: "Birthdate");

            migrationBuilder.CreateIndex(
                name: "Index_Livestock_Deleted",
                table: "Livestock",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "Index_Livestock_ModifiedOn",
                table: "Livestock",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "Index_Livestock_TenantIdAndPrimaryKey",
                table: "Livestock",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "Index_Name",
                table: "Livestock",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_BreedId",
                table: "Livestock",
                column: "BreedId");

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_FatherId",
                table: "Livestock",
                column: "FatherId",
                unique: true,
                filter: "[FatherId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_MotherId",
                table: "Livestock",
                column: "MotherId",
                unique: true,
                filter: "[MotherId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockBreed_LivestockId",
                table: "LivestockBreed",
                column: "LivestockId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockFeed_LivestockTypeId",
                table: "LivestockFeed",
                column: "LivestockTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockLivestockStatus_StatusesId",
                table: "LivestockLivestockStatus",
                column: "StatusesId");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockStatus_Deleted",
                table: "LivestockStatus",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockStatus_ModifiedOn",
                table: "LivestockStatus",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockStatus_TenantIdAndPrimaryKey",
                table: "LivestockStatus",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_LivestockStatus_LivestockTypeId",
                table: "LivestockStatus",
                column: "LivestockTypeId");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockType_Deleted",
                table: "LivestockType",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockType_ModifiedOn",
                table: "LivestockType",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "Index_LivestockType_TenantIdAndPrimaryKey",
                table: "LivestockType",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_LivestockType_Name",
                table: "LivestockType",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LivestockFeed");

            migrationBuilder.DropTable(
                name: "LivestockLivestockStatus");

            migrationBuilder.DropTable(
                name: "LivestockStatus");

            migrationBuilder.DropTable(
                name: "Livestock");

            migrationBuilder.DropTable(
                name: "LivestockBreed");

            migrationBuilder.DropTable(
                name: "LivestockType");
        }
    }
}
