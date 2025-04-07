﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiquorStore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccountss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Account");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Account",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
