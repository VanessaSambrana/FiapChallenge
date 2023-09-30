using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Procurados.Migrations
{
    /// <inheritdoc />
    public partial class CriandoTabelaDeProcurados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_USERS",
                columns: table => new
                {
                    ID_USER = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_NOME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NM_EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    SEC_PASSWORD = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_USERS", x => x.ID_USER);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_USERS");
        }
    }
}
