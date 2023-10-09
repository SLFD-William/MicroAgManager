using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Treatment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    BrandName = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Reason = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    LabelMethod = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    MeatWithdrawal = table.Column<int>(type: "INTEGER", nullable: false),
                    MilkWithdrawal = table.Column<int>(type: "INTEGER", nullable: false),
                    DosageAmount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 3, nullable: false),
                    DosageUnitId = table.Column<long>(type: "INTEGER", nullable: false),
                    AnimalMass = table.Column<decimal>(type: "TEXT", precision: 18, scale: 3, nullable: false),
                    AnimalMassUnitId = table.Column<long>(type: "INTEGER", nullable: false),
                    Frequency = table.Column<decimal>(type: "TEXT", precision: 18, scale: 3, nullable: false),
                    FrequencyUnitId = table.Column<long>(type: "INTEGER", nullable: false),
                    Duration = table.Column<decimal>(type: "TEXT", precision: 18, scale: 3, nullable: false),
                    DurationUnitId = table.Column<long>(type: "INTEGER", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Treatments_Units_AnimalMassUnitId",
                        column: x => x.AnimalMassUnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Treatments_Units_DosageUnitId",
                        column: x => x.DosageUnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Treatments_Units_DurationUnitId",
                        column: x => x.DurationUnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Treatments_Units_FrequencyUnitId",
                        column: x => x.FrequencyUnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentRecords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TreatmentId = table.Column<long>(type: "INTEGER", nullable: false),
                    RecipientTypeId = table.Column<long>(type: "INTEGER", nullable: false),
                    RecipientType = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    RecipientId = table.Column<long>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    DatePerformed = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DosageAmount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 3, nullable: false),
                    DosageUnitId = table.Column<long>(type: "INTEGER", nullable: false),
                    AppliedMethod = table.Column<string>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreatmentRecords_Treatments_TreatmentId",
                        column: x => x.TreatmentId,
                        principalTable: "Treatments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreatmentRecords_Units_DosageUnitId",
                        column: x => x.DosageUnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentRecords_DosageUnitId",
                table: "TreatmentRecords",
                column: "DosageUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentRecords_TreatmentId",
                table: "TreatmentRecords",
                column: "TreatmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_AnimalMassUnitId",
                table: "Treatments",
                column: "AnimalMassUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_DosageUnitId",
                table: "Treatments",
                column: "DosageUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_DurationUnitId",
                table: "Treatments",
                column: "DurationUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_FrequencyUnitId",
                table: "Treatments",
                column: "FrequencyUnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TreatmentRecords");

            migrationBuilder.DropTable(
                name: "Treatments");
        }
    }
}
