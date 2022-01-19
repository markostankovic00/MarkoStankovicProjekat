using Microsoft.EntityFrameworkCore.Migrations;

namespace Projekat.Migrations
{
    public partial class V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Klijenti",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    brKartice = table.Column<int>(type: "int", nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klijenti", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Teretane",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CenaPoSatu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teretane", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Treninzi",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Duzina = table.Column<int>(type: "int", nullable: false),
                    Tip = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treninzi", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "KlijentiTreninzi",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cena = table.Column<int>(type: "int", nullable: false),
                    TeretanaID = table.Column<int>(type: "int", nullable: true),
                    KlijentID = table.Column<int>(type: "int", nullable: true),
                    TreningID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KlijentiTreninzi", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KlijentiTreninzi_Klijenti_KlijentID",
                        column: x => x.KlijentID,
                        principalTable: "Klijenti",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KlijentiTreninzi_Teretane_TeretanaID",
                        column: x => x.TeretanaID,
                        principalTable: "Teretane",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KlijentiTreninzi_Treninzi_TreningID",
                        column: x => x.TreningID,
                        principalTable: "Treninzi",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KlijentiTreninzi_KlijentID",
                table: "KlijentiTreninzi",
                column: "KlijentID");

            migrationBuilder.CreateIndex(
                name: "IX_KlijentiTreninzi_TeretanaID",
                table: "KlijentiTreninzi",
                column: "TeretanaID");

            migrationBuilder.CreateIndex(
                name: "IX_KlijentiTreninzi_TreningID",
                table: "KlijentiTreninzi",
                column: "TreningID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KlijentiTreninzi");

            migrationBuilder.DropTable(
                name: "Klijenti");

            migrationBuilder.DropTable(
                name: "Teretane");

            migrationBuilder.DropTable(
                name: "Treninzi");
        }
    }
}
