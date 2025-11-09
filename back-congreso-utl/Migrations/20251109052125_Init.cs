using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace back_congreso_utl.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "participantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Apellidos = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Twitter = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Ocupacion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Avatar = table.Column<string>(type: "text", nullable: true),
                    AceptaTerminos = table.Column<bool>(type: "boolean", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_participantes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "participantes",
                columns: new[] { "Id", "AceptaTerminos", "Apellidos", "Avatar", "Email", "FechaRegistro", "Nombre", "Ocupacion", "Twitter" },
                values: new object[,]
                {
                    { 1, true, "Rubio", "👨‍💻", "jrubio@mail.com", null, "Julián", "Desarrollador de Software", "JRubio" },
                    { 2, true, "Medina", "👨‍💼", "rmedina@mail.com", null, "Raúl", "Ingeniero Front End", "RaulMedina" },
                    { 3, true, "Andrade", "🧑‍💻", "candrade@mail.com", null, "Carlos", "Desarrollador Web Full Stack", "CAndrade" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "participantes");
        }
    }
}
