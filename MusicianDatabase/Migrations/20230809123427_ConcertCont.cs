using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicianDatabase.Migrations
{
    public partial class ConcertCont : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Concert_Venue_VenueId",
                table: "Concert");

            migrationBuilder.DropForeignKey(
                name: "FK_ConcertBand_Bands_BandId",
                table: "ConcertBand");

            migrationBuilder.DropForeignKey(
                name: "FK_ConcertBand_Concert_ConcertId",
                table: "ConcertBand");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Venue",
                table: "Venue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConcertBand",
                table: "ConcertBand");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Concert",
                table: "Concert");

            migrationBuilder.RenameTable(
                name: "Venue",
                newName: "Venues");

            migrationBuilder.RenameTable(
                name: "ConcertBand",
                newName: "ConcertBands");

            migrationBuilder.RenameTable(
                name: "Concert",
                newName: "Concerts");

            migrationBuilder.RenameIndex(
                name: "IX_ConcertBand_BandId",
                table: "ConcertBands",
                newName: "IX_ConcertBands_BandId");

            migrationBuilder.RenameIndex(
                name: "IX_Concert_VenueId",
                table: "Concerts",
                newName: "IX_Concerts_VenueId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Venues",
                table: "Venues",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConcertBands",
                table: "ConcertBands",
                columns: new[] { "ConcertId", "BandId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Concerts",
                table: "Concerts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConcertBands_Bands_BandId",
                table: "ConcertBands",
                column: "BandId",
                principalTable: "Bands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConcertBands_Concerts_ConcertId",
                table: "ConcertBands",
                column: "ConcertId",
                principalTable: "Concerts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Concerts_Venues_VenueId",
                table: "Concerts",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConcertBands_Bands_BandId",
                table: "ConcertBands");

            migrationBuilder.DropForeignKey(
                name: "FK_ConcertBands_Concerts_ConcertId",
                table: "ConcertBands");

            migrationBuilder.DropForeignKey(
                name: "FK_Concerts_Venues_VenueId",
                table: "Concerts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Venues",
                table: "Venues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Concerts",
                table: "Concerts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConcertBands",
                table: "ConcertBands");

            migrationBuilder.RenameTable(
                name: "Venues",
                newName: "Venue");

            migrationBuilder.RenameTable(
                name: "Concerts",
                newName: "Concert");

            migrationBuilder.RenameTable(
                name: "ConcertBands",
                newName: "ConcertBand");

            migrationBuilder.RenameIndex(
                name: "IX_Concerts_VenueId",
                table: "Concert",
                newName: "IX_Concert_VenueId");

            migrationBuilder.RenameIndex(
                name: "IX_ConcertBands_BandId",
                table: "ConcertBand",
                newName: "IX_ConcertBand_BandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Venue",
                table: "Venue",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Concert",
                table: "Concert",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConcertBand",
                table: "ConcertBand",
                columns: new[] { "ConcertId", "BandId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Concert_Venue_VenueId",
                table: "Concert",
                column: "VenueId",
                principalTable: "Venue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConcertBand_Bands_BandId",
                table: "ConcertBand",
                column: "BandId",
                principalTable: "Bands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConcertBand_Concert_ConcertId",
                table: "ConcertBand",
                column: "ConcertId",
                principalTable: "Concert",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
