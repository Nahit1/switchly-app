using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Switchly.WebApi.Context.Migrations
{
    /// <inheritdoc />
    public partial class addNewColumnForProjectEnvironmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "ProjectEnvironments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "ProjectEnvironments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "ProjectEnvironments");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "ProjectEnvironments");
        }
    }
}
