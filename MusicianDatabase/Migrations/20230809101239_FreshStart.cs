using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicianDatabase.Migrations
{
    public partial class FreshStart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineupBands");

            migrationBuilder.DropTable(
                name: "Lineup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleInstruments",
                table: "RoleInstruments");

            migrationBuilder.DropIndex(
                name: "IX_RoleInstruments_RoleId",
                table: "RoleInstruments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RoleInstruments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleInstruments",
                table: "RoleInstruments",
                columns: new[] { "RoleId", "InstrumentId" });

            migrationBuilder.CreateTable(
                name: "ConcertBand",
                columns: table => new
                {
                    ConcertId = table.Column<int>(type: "int", nullable: false),
                    BandId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcertBand", x => new { x.ConcertId, x.BandId });
                    table.ForeignKey(
                        name: "FK_ConcertBand_Bands_BandId",
                        column: x => x.BandId,
                        principalTable: "Bands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConcertBand_Concert_ConcertId",
                        column: x => x.ConcertId,
                        principalTable: "Concert",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConcertBand_BandId",
                table: "ConcertBand",
                column: "BandId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConcertBand");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleInstruments",
                table: "RoleInstruments");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "RoleInstruments",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleInstruments",
                table: "RoleInstruments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Lineup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConcertId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lineup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lineup_Concert_ConcertId",
                        column: x => x.ConcertId,
                        principalTable: "Concert",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LineupBands",
                columns: table => new
                {
                    LineupId = table.Column<int>(type: "int", nullable: false),
                    BandId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineupBands", x => new { x.LineupId, x.BandId });
                    table.ForeignKey(
                        name: "FK_LineupBands_Bands_BandId",
                        column: x => x.BandId,
                        principalTable: "Bands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineupBands_Lineup_LineupId",
                        column: x => x.LineupId,
                        principalTable: "Lineup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleInstruments_RoleId",
                table: "RoleInstruments",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Lineup_ConcertId",
                table: "Lineup",
                column: "ConcertId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LineupBands_BandId",
                table: "LineupBands",
                column: "BandId");
        }
    }
}
