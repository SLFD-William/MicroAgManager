using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class LivestockRemoveEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livestock_LivestockBreed_BreedId",
                table: "Livestock");

            migrationBuilder.DropForeignKey(
                name: "FK_Livestock_Livestock_FatherId",
                table: "Livestock");

            migrationBuilder.DropForeignKey(
                name: "FK_Livestock_Livestock_MotherId",
                table: "Livestock");

            migrationBuilder.DropForeignKey(
                name: "FK_LivestockBreed_LivestockType_LivestockId",
                table: "LivestockBreed");

            migrationBuilder.DropForeignKey(
                name: "FK_LivestockFeed_LivestockType_LivestockTypeId",
                table: "LivestockFeed");

            migrationBuilder.DropForeignKey(
                name: "FK_LivestockLivestockStatus_LivestockStatus_StatusesId",
                table: "LivestockLivestockStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_LivestockLivestockStatus_Livestock_LivestocksId",
                table: "LivestockLivestockStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_LivestockStatus_LivestockType_LivestockTypeId",
                table: "LivestockStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LivestockType",
                table: "LivestockType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LivestockStatus",
                table: "LivestockStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LivestockFeed",
                table: "LivestockFeed");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LivestockBreed",
                table: "LivestockBreed");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Livestock",
                table: "Livestock");

            migrationBuilder.RenameTable(
                name: "LivestockType",
                newName: "LivestockTypes");

            migrationBuilder.RenameTable(
                name: "LivestockStatus",
                newName: "LivestockStatuses");

            migrationBuilder.RenameTable(
                name: "LivestockFeed",
                newName: "LivestockFeeds");

            migrationBuilder.RenameTable(
                name: "LivestockBreed",
                newName: "LivestockBreeds");

            migrationBuilder.RenameTable(
                name: "Livestock",
                newName: "Livestocks");

            migrationBuilder.RenameIndex(
                name: "IX_LivestockType_Name",
                table: "LivestockTypes",
                newName: "IX_LivestockTypes_Name");

            migrationBuilder.RenameIndex(
                name: "IX_LivestockStatus_LivestockTypeId",
                table: "LivestockStatuses",
                newName: "IX_LivestockStatuses_LivestockTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_LivestockFeed_LivestockTypeId",
                table: "LivestockFeeds",
                newName: "IX_LivestockFeeds_LivestockTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_LivestockBreed_LivestockId",
                table: "LivestockBreeds",
                newName: "IX_LivestockBreeds_LivestockId");

            migrationBuilder.RenameIndex(
                name: "IX_Livestock_MotherId",
                table: "Livestocks",
                newName: "IX_Livestocks_MotherId");

            migrationBuilder.RenameIndex(
                name: "IX_Livestock_FatherId",
                table: "Livestocks",
                newName: "IX_Livestocks_FatherId");

            migrationBuilder.RenameIndex(
                name: "IX_Livestock_BreedId",
                table: "Livestocks",
                newName: "IX_Livestocks_BreedId");

            migrationBuilder.AlterColumn<string>(
                name: "Usage",
                table: "Plots",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Care",
                table: "LivestockTypes",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Sterile",
                table: "LivestockStatuses",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "InMilk",
                table: "LivestockStatuses",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "ForSale",
                table: "LivestockStatuses",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "BottleFed",
                table: "LivestockStatuses",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "BeingManaged",
                table: "LivestockStatuses",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "LivestockFeeds",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Cutting",
                table: "LivestockFeeds",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FeedType",
                table: "LivestockFeeds",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "LivestockFeeds",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "LivestockFeeds",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "QuantityUnit",
                table: "LivestockFeeds",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "QuantityWarning",
                table: "LivestockFeeds",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "LivestockFeeds",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LivestockBreeds",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EmojiChar",
                table: "LivestockBreeds",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LivestockTypes",
                table: "LivestockTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LivestockStatuses",
                table: "LivestockStatuses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LivestockFeeds",
                table: "LivestockFeeds",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LivestockBreeds",
                table: "LivestockBreeds",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Livestocks",
                table: "Livestocks",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_LivestockBreeds_LivestockTypes_LivestockId",
                table: "LivestockBreeds",
                column: "LivestockId",
                principalTable: "LivestockTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LivestockFeeds_LivestockTypes_LivestockTypeId",
                table: "LivestockFeeds",
                column: "LivestockTypeId",
                principalTable: "LivestockTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LivestockLivestockStatus_LivestockStatuses_StatusesId",
                table: "LivestockLivestockStatus",
                column: "StatusesId",
                principalTable: "LivestockStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction,
                onUpdate:ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LivestockLivestockStatus_Livestocks_LivestocksId",
                table: "LivestockLivestockStatus",
                column: "LivestocksId",
                principalTable: "Livestocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction,
                onUpdate: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Livestocks_LivestockBreeds_BreedId",
                table: "Livestocks",
                column: "BreedId",
                principalTable: "LivestockBreeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Livestocks_Livestocks_FatherId",
                table: "Livestocks",
                column: "FatherId",
                principalTable: "Livestocks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Livestocks_Livestocks_MotherId",
                table: "Livestocks",
                column: "MotherId",
                principalTable: "Livestocks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LivestockStatuses_LivestockTypes_LivestockTypeId",
                table: "LivestockStatuses",
                column: "LivestockTypeId",
                principalTable: "LivestockTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LivestockBreeds_LivestockTypes_LivestockId",
                table: "LivestockBreeds");

            migrationBuilder.DropForeignKey(
                name: "FK_LivestockFeeds_LivestockTypes_LivestockTypeId",
                table: "LivestockFeeds");

            migrationBuilder.DropForeignKey(
                name: "FK_LivestockLivestockStatus_LivestockStatuses_StatusesId",
                table: "LivestockLivestockStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_LivestockLivestockStatus_Livestocks_LivestocksId",
                table: "LivestockLivestockStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_Livestocks_LivestockBreeds_BreedId",
                table: "Livestocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Livestocks_Livestocks_FatherId",
                table: "Livestocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Livestocks_Livestocks_MotherId",
                table: "Livestocks");

            migrationBuilder.DropForeignKey(
                name: "FK_LivestockStatuses_LivestockTypes_LivestockTypeId",
                table: "LivestockStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LivestockTypes",
                table: "LivestockTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LivestockStatuses",
                table: "LivestockStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Livestocks",
                table: "Livestocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LivestockFeeds",
                table: "LivestockFeeds");

            migrationBuilder.DropIndex(
                name: "Index_LivestockFeed_Deleted",
                table: "LivestockFeeds");

            migrationBuilder.DropIndex(
                name: "Index_LivestockFeed_ModifiedOn",
                table: "LivestockFeeds");

            migrationBuilder.DropIndex(
                name: "Index_LivestockFeed_TenantIdAndPrimaryKey",
                table: "LivestockFeeds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LivestockBreeds",
                table: "LivestockBreeds");

            migrationBuilder.DropIndex(
                name: "Index_LivestockBreed_Deleted",
                table: "LivestockBreeds");

            migrationBuilder.DropIndex(
                name: "Index_LivestockBreed_ModifiedOn",
                table: "LivestockBreeds");

            migrationBuilder.DropIndex(
                name: "Index_LivestockBreed_TenantIdAndPrimaryKey",
                table: "LivestockBreeds");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "LivestockFeeds");

            migrationBuilder.DropColumn(
                name: "Cutting",
                table: "LivestockFeeds");

            migrationBuilder.DropColumn(
                name: "FeedType",
                table: "LivestockFeeds");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "LivestockFeeds");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "LivestockFeeds");

            migrationBuilder.DropColumn(
                name: "QuantityUnit",
                table: "LivestockFeeds");

            migrationBuilder.DropColumn(
                name: "QuantityWarning",
                table: "LivestockFeeds");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "LivestockFeeds");

            migrationBuilder.RenameTable(
                name: "LivestockTypes",
                newName: "LivestockType");

            migrationBuilder.RenameTable(
                name: "LivestockStatuses",
                newName: "LivestockStatus");

            migrationBuilder.RenameTable(
                name: "Livestocks",
                newName: "Livestock");

            migrationBuilder.RenameTable(
                name: "LivestockFeeds",
                newName: "LivestockFeed");

            migrationBuilder.RenameTable(
                name: "LivestockBreeds",
                newName: "LivestockBreed");

            migrationBuilder.RenameIndex(
                name: "IX_LivestockTypes_Name",
                table: "LivestockType",
                newName: "IX_LivestockType_Name");

            migrationBuilder.RenameIndex(
                name: "IX_LivestockStatuses_LivestockTypeId",
                table: "LivestockStatus",
                newName: "IX_LivestockStatus_LivestockTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Livestocks_MotherId",
                table: "Livestock",
                newName: "IX_Livestock_MotherId");

            migrationBuilder.RenameIndex(
                name: "IX_Livestocks_FatherId",
                table: "Livestock",
                newName: "IX_Livestock_FatherId");

            migrationBuilder.RenameIndex(
                name: "IX_Livestocks_BreedId",
                table: "Livestock",
                newName: "IX_Livestock_BreedId");

            migrationBuilder.RenameIndex(
                name: "IX_LivestockFeeds_LivestockTypeId",
                table: "LivestockFeed",
                newName: "IX_LivestockFeed_LivestockTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_LivestockBreeds_LivestockId",
                table: "LivestockBreed",
                newName: "IX_LivestockBreed_LivestockId");

            migrationBuilder.AlterColumn<long>(
                name: "Usage",
                table: "Plots",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<long>(
                name: "Care",
                table: "LivestockType",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40);

            migrationBuilder.AlterColumn<long>(
                name: "Sterile",
                table: "LivestockStatus",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<long>(
                name: "InMilk",
                table: "LivestockStatus",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<long>(
                name: "ForSale",
                table: "LivestockStatus",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<long>(
                name: "BottleFed",
                table: "LivestockStatus",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<long>(
                name: "BeingManaged",
                table: "LivestockStatus",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LivestockBreed",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40);

            migrationBuilder.AlterColumn<string>(
                name: "EmojiChar",
                table: "LivestockBreed",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2)",
                oldMaxLength: 2);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LivestockType",
                table: "LivestockType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LivestockStatus",
                table: "LivestockStatus",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Livestock",
                table: "Livestock",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LivestockFeed",
                table: "LivestockFeed",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LivestockBreed",
                table: "LivestockBreed",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Livestock_LivestockBreed_BreedId",
                table: "Livestock",
                column: "BreedId",
                principalTable: "LivestockBreed",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Livestock_Livestock_FatherId",
                table: "Livestock",
                column: "FatherId",
                principalTable: "Livestock",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Livestock_Livestock_MotherId",
                table: "Livestock",
                column: "MotherId",
                principalTable: "Livestock",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LivestockBreed_LivestockType_LivestockId",
                table: "LivestockBreed",
                column: "LivestockId",
                principalTable: "LivestockType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LivestockFeed_LivestockType_LivestockTypeId",
                table: "LivestockFeed",
                column: "LivestockTypeId",
                principalTable: "LivestockType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LivestockLivestockStatus_LivestockStatus_StatusesId",
                table: "LivestockLivestockStatus",
                column: "StatusesId",
                principalTable: "LivestockStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LivestockLivestockStatus_Livestock_LivestocksId",
                table: "LivestockLivestockStatus",
                column: "LivestocksId",
                principalTable: "Livestock",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LivestockStatus_LivestockType_LivestockTypeId",
                table: "LivestockStatus",
                column: "LivestockTypeId",
                principalTable: "LivestockType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
