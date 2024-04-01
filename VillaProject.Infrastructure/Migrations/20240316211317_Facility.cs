using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VillaProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Facility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacilityVilla_Facility_FacilitiesId",
                table: "FacilityVilla");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Facility",
                table: "Facility");

            migrationBuilder.RenameTable(
                name: "Facility",
                newName: "Facilities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Facilities",
                table: "Facilities",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "VillaFacilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacilityId = table.Column<int>(type: "int", nullable: false),
                    VillaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VillaFacilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VillaFacilities_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VillaFacilities_Villas_VillaId",
                        column: x => x.VillaId,
                        principalTable: "Villas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VillaFacilities_FacilityId",
                table: "VillaFacilities",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_VillaFacilities_VillaId",
                table: "VillaFacilities",
                column: "VillaId");

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityVilla_Facilities_FacilitiesId",
                table: "FacilityVilla",
                column: "FacilitiesId",
                principalTable: "Facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacilityVilla_Facilities_FacilitiesId",
                table: "FacilityVilla");

            migrationBuilder.DropTable(
                name: "VillaFacilities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Facilities",
                table: "Facilities");

            migrationBuilder.RenameTable(
                name: "Facilities",
                newName: "Facility");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Facility",
                table: "Facility",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityVilla_Facility_FacilitiesId",
                table: "FacilityVilla",
                column: "FacilitiesId",
                principalTable: "Facility",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
