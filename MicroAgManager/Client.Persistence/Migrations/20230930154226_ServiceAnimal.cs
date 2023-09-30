using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ServiceAnimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BreedingRecords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FemaleId = table.Column<long>(type: "INTEGER", nullable: false),
                    MaleId = table.Column<long>(type: "INTEGER", nullable: true),
                    ServiceDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ResolutionDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StillbornMales = table.Column<int>(type: "INTEGER", nullable: true),
                    StillbornFemales = table.Column<int>(type: "INTEGER", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    EntityModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreedingRecords", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BreedingRecords");
        }
    }
}
