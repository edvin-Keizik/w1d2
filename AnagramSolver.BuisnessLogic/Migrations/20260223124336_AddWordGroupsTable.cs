using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnagramSolver.BusinessLogic.Migrations
{
    /// <inheritdoc />
    public partial class AddWordGroupsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WordGroupsEntity",
                columns: table => new
                {
                    Signature = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Words = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordGroupsEntity", x => x.Signature);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordGroupsEntity");
        }
    }
}
