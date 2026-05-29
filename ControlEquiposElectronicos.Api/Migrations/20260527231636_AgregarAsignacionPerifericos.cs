using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlEquiposElectronicos.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarAsignacionPerifericos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipoPerifericos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipoPrincipalId = table.Column<int>(type: "int", nullable: false),
                    PerifericoId = table.Column<int>(type: "int", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaDesasignacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipoPerifericos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipoPerifericos_Equipos_EquipoPrincipalId",
                        column: x => x.EquipoPrincipalId,
                        principalTable: "Equipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipoPerifericos_Equipos_PerifericoId",
                        column: x => x.PerifericoId,
                        principalTable: "Equipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipoPerifericos_EquipoPrincipal_Periferico_Activo",
                table: "EquipoPerifericos",
                columns: new[] { "EquipoPrincipalId", "PerifericoId", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_EquipoPerifericos_PerifericoId",
                table: "EquipoPerifericos",
                column: "PerifericoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipoPerifericos");
        }
    }
}
