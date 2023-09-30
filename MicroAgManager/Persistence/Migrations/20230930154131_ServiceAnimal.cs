using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ServiceAnimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledDuties_Livestocks_LivestockId",
                table: "ScheduledDuties");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledDuties_LivestockId",
                table: "ScheduledDuties");

            migrationBuilder.DropColumn(
                name: "LivestockId",
                table: "ScheduledDuties");

            migrationBuilder.CreateTable(
                name: "BreedingRecords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FemaleId = table.Column<long>(type: "bigint", nullable: false),
                    MaleId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResolutionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StillbornMales = table.Column<int>(type: "int", nullable: true),
                    StillbornFemales = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreedingRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BreedingRecords_Livestocks_FemaleId",
                        column: x => x.FemaleId,
                        principalTable: "Livestocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BreedingRecords_Livestocks_MaleId",
                        column: x => x.MaleId,
                        principalTable: "Livestocks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BreedingRecords_FemaleId",
                table: "BreedingRecords",
                column: "FemaleId");

            migrationBuilder.CreateIndex(
                name: "IX_BreedingRecords_MaleId",
                table: "BreedingRecords",
                column: "MaleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BreedingRecords");

            migrationBuilder.AddColumn<long>(
                name: "LivestockId",
                table: "ScheduledDuties",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDuties_LivestockId",
                table: "ScheduledDuties",
                column: "LivestockId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledDuties_Livestocks_LivestockId",
                table: "ScheduledDuties",
                column: "LivestockId",
                principalTable: "Livestocks",
                principalColumn: "Id");
        }
    }
}
