using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProteinCase.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrencyCode = table.Column<string>(type: "text", nullable: true),
                    Unit = table.Column<int>(type: "integer", nullable: false),
                    Isim = table.Column<string>(type: "text", nullable: true),
                    CurrencyName = table.Column<string>(type: "text", nullable: true),
                    ForexBuying = table.Column<decimal>(type: "numeric", nullable: false),
                    ForexSelling = table.Column<decimal>(type: "numeric", nullable: false),
                    BanknoteBuying = table.Column<decimal>(type: "numeric", nullable: true),
                    BanknoteSelling = table.Column<decimal>(type: "numeric", nullable: true),
                    CrossRateUSD = table.Column<decimal>(type: "numeric", nullable: true),
                    CrossRateOther = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
