using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Conciliator.App.Migrations
{
    public partial class AddExtract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Extracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TransactionType = table.Column<string>(type: "varchar(50)", nullable: false),
                    DatePosted = table.Column<DateTime>(type: "datetime", nullable: false),
                    TransactionAmount = table.Column<decimal>(type: "decimal(13, 2)", nullable: false),
                    Memo = table.Column<string>(type: "varchar(50)", nullable: false),
                    Note = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Extracts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Extracts");
        }
    }
}
