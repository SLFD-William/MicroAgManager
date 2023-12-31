using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddingChores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ChoreId",
                table: "Duties",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Chores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientTypeId = table.Column<long>(type: "bigint", nullable: false),
                    RecipientType = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    DueByTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Singularly = table.Column<bool>(type: "bit", nullable: false),
                    Frequency = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    FrequencyUnitId = table.Column<long>(type: "bigint", nullable: false),
                    Period = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    PeriodUnitId = table.Column<long>(type: "bigint", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chores_Units_FrequencyUnitId",
                        column: x => x.FrequencyUnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chores_Units_PeriodUnitId",
                        column: x => x.PeriodUnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Duties_ChoreId",
                table: "Duties",
                column: "ChoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Chores_FrequencyUnitId",
                table: "Chores",
                column: "FrequencyUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Chores_ModifiedOn",
                table: "Chores",
                column: "ModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Chores_PeriodUnitId",
                table: "Chores",
                column: "PeriodUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Chores_TenantId",
                table: "Chores",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Duties_Chores_ChoreId",
                table: "Duties",
                column: "ChoreId",
                principalTable: "Chores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Duties_Chores_ChoreId",
                table: "Duties");

            migrationBuilder.DropTable(
                name: "Chores");

            migrationBuilder.DropIndex(
                name: "IX_Duties_ChoreId",
                table: "Duties");

            migrationBuilder.DropColumn(
                name: "ChoreId",
                table: "Duties");
        }
    }
}
