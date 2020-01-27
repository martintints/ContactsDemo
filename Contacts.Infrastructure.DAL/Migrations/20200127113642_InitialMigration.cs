using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Contacts.Infrastructure.DAL.Migrations
{
    [SuppressMessage("ReSharper", "RedundantArgumentDefaultValue")]
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(maxLength: 100, nullable: false),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    Email = table.Column<string>(maxLength: 254, nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    Sequence = table.Column<decimal>(type: "NUMERIC(32,16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Contact",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Phone", "Sequence" },
                values: new object[] { 1, "juhan.juurikas@gmail.com", "Juhan", "Juurikas", "+3725123456", 4000m });

            migrationBuilder.InsertData(
                table: "Contact",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Phone", "Sequence" },
                values: new object[] { 2, "mari.maasikas@gmail.com", "Mari", "Maasikas", "+3725223456", 2000m });

            migrationBuilder.InsertData(
                table: "Contact",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Phone", "Sequence" },
                values: new object[] { 3, "john.doe@gmail.com", "John", "Doe", "+1-202-555-0139", 1000m });

            migrationBuilder.InsertData(
                table: "Contact",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Phone", "Sequence" },
                values: new object[] { 4, "jane.doe@gmail.com", "Jane", "Doe", "+1-202-555-0182", 9000m });

            migrationBuilder.CreateIndex(
                name: "IX_Contact_Sequence",
                table: "Contact",
                column: "Sequence");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contact");
        }
    }
}
