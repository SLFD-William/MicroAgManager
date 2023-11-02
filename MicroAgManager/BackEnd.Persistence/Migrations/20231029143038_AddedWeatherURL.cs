﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedWeatherURL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WeatherServiceQueryURL",
                table: "Tenants",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeatherServiceQueryURL",
                table: "Tenants");
        }
    }
}