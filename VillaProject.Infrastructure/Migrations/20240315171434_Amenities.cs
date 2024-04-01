using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VillaProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Amenities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Restraunt",
                table: "Amenities",
                newName: "Restaurant");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Restaurant",
                table: "Amenities",
                newName: "Restraunt");
        }
    }
}
