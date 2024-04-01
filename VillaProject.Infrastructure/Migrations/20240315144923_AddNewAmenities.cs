using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VillaProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewAmenities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Amenities");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Amenities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "AC",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Balcony",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Breakfast",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DeliveryService",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Freezer",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "GardenView",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HDTV",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Heating",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "KingBed",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Kitchen",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Microwave",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MountainView",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OceanView",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Parking",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Pool",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Restraunt",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Safe",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SelfCheckIn",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Wifi",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AC", "Balcony", "Breakfast", "DeliveryService", "Freezer", "GardenView", "HDTV", "Heating", "KingBed", "Kitchen", "Microwave", "MountainView", "Name", "OceanView", "Parking", "Pool", "Restraunt", "Safe", "SelfCheckIn", "Wifi" },
                values: new object[] { false, false, false, false, false, false, false, false, false, false, false, false, null, false, false, false, false, false, false, false });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AC", "Balcony", "Breakfast", "DeliveryService", "Freezer", "GardenView", "HDTV", "Heating", "KingBed", "Kitchen", "Microwave", "MountainView", "Name", "OceanView", "Parking", "Pool", "Restraunt", "Safe", "SelfCheckIn", "Wifi" },
                values: new object[] { false, false, false, false, false, false, false, false, false, false, false, false, null, false, false, false, false, false, false, false });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AC", "Balcony", "Breakfast", "DeliveryService", "Freezer", "GardenView", "HDTV", "Heating", "KingBed", "Kitchen", "Microwave", "MountainView", "Name", "OceanView", "Parking", "Pool", "Restraunt", "Safe", "SelfCheckIn", "Wifi" },
                values: new object[] { false, false, false, false, false, false, false, false, false, false, false, false, null, false, false, false, false, false, false, false });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AC", "Balcony", "Breakfast", "DeliveryService", "Freezer", "GardenView", "HDTV", "Heating", "KingBed", "Kitchen", "Microwave", "MountainView", "Name", "OceanView", "Parking", "Pool", "Restraunt", "Safe", "SelfCheckIn", "Wifi" },
                values: new object[] { false, false, false, false, false, false, false, false, false, false, false, false, null, false, false, false, false, false, false, false });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AC", "Balcony", "Breakfast", "DeliveryService", "Freezer", "GardenView", "HDTV", "Heating", "KingBed", "Kitchen", "Microwave", "MountainView", "Name", "OceanView", "Parking", "Pool", "Restraunt", "Safe", "SelfCheckIn", "Wifi" },
                values: new object[] { false, false, false, false, false, false, false, false, false, false, false, false, null, false, false, false, false, false, false, false });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AC", "Balcony", "Breakfast", "DeliveryService", "Freezer", "GardenView", "HDTV", "Heating", "KingBed", "Kitchen", "Microwave", "MountainView", "Name", "OceanView", "Parking", "Pool", "Restraunt", "Safe", "SelfCheckIn", "Wifi" },
                values: new object[] { false, false, false, false, false, false, false, false, false, false, false, false, null, false, false, false, false, false, false, false });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "AC", "Balcony", "Breakfast", "DeliveryService", "Freezer", "GardenView", "HDTV", "Heating", "KingBed", "Kitchen", "Microwave", "MountainView", "Name", "OceanView", "Parking", "Pool", "Restraunt", "Safe", "SelfCheckIn", "Wifi" },
                values: new object[] { false, false, false, false, false, false, false, false, false, false, false, false, null, false, false, false, false, false, false, false });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "AC", "Balcony", "Breakfast", "DeliveryService", "Freezer", "GardenView", "HDTV", "Heating", "KingBed", "Kitchen", "Microwave", "MountainView", "Name", "OceanView", "Parking", "Pool", "Restraunt", "Safe", "SelfCheckIn", "Wifi" },
                values: new object[] { false, false, false, false, false, false, false, false, false, false, false, false, null, false, false, false, false, false, false, false });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "AC", "Balcony", "Breakfast", "DeliveryService", "Freezer", "GardenView", "HDTV", "Heating", "KingBed", "Kitchen", "Microwave", "MountainView", "Name", "OceanView", "Parking", "Pool", "Restraunt", "Safe", "SelfCheckIn", "Wifi" },
                values: new object[] { false, false, false, false, false, false, false, false, false, false, false, false, null, false, false, false, false, false, false, false });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "AC", "Balcony", "Breakfast", "DeliveryService", "Freezer", "GardenView", "HDTV", "Heating", "KingBed", "Kitchen", "Microwave", "MountainView", "Name", "OceanView", "Parking", "Pool", "Restraunt", "Safe", "SelfCheckIn", "Wifi" },
                values: new object[] { false, false, false, false, false, false, false, false, false, false, false, false, null, false, false, false, false, false, false, false });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "AC", "Balcony", "Breakfast", "DeliveryService", "Freezer", "GardenView", "HDTV", "Heating", "KingBed", "Kitchen", "Microwave", "MountainView", "Name", "OceanView", "Parking", "Pool", "Restraunt", "Safe", "SelfCheckIn", "Wifi" },
                values: new object[] { false, false, false, false, false, false, false, false, false, false, false, false, null, false, false, false, false, false, false, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AC",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "Balcony",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "Breakfast",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "DeliveryService",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "Freezer",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "GardenView",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "HDTV",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "Heating",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "KingBed",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "Kitchen",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "Microwave",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "MountainView",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "OceanView",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "Parking",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "Pool",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "Restraunt",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "Safe",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "SelfCheckIn",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "Wifi",
                table: "Amenities");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Amenities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Amenities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { null, "Private Pool" });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { null, "Microwave" });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Name" },
                values: new object[] { null, "Private Balcony" });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Name" },
                values: new object[] { null, "1 king bed and 1 sofa bed" });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Name" },
                values: new object[] { null, "Private Plunge Pool" });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Description", "Name" },
                values: new object[] { null, "Microwave and Mini Refrigerator" });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Description", "Name" },
                values: new object[] { null, "Private Balcony" });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Description", "Name" },
                values: new object[] { null, "king bed or 2 double beds" });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Description", "Name" },
                values: new object[] { null, "Private Pool" });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Description", "Name" },
                values: new object[] { null, "Jacuzzi" });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Description", "Name" },
                values: new object[] { null, "Private Balcony" });
        }
    }
}
