using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlEquiposElectronicos.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarModuloChecklistTecnico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChecklistTecnicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UbicacionId = table.Column<int>(type: "int", nullable: false),
                    TecnicoId = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaFinalizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstadoChecklist = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ObservacionesGenerales = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistTecnicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChecklistTecnicos_Ubicaciones_UbicacionId",
                        column: x => x.UbicacionId,
                        principalTable: "Ubicaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChecklistTecnicos_Usuarios_TecnicoId",
                        column: x => x.TecnicoId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlantillasChecklist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CategoriaEquipoId = table.Column<int>(type: "int", nullable: true),
                    TipoEquipoId = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantillasChecklist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantillasChecklist_CategoriasEquipo_CategoriaEquipoId",
                        column: x => x.CategoriaEquipoId,
                        principalTable: "CategoriasEquipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlantillasChecklist_TiposEquipo_TipoEquipoId",
                        column: x => x.TipoEquipoId,
                        principalTable: "TiposEquipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistTecnicoEquipos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChecklistTecnicoId = table.Column<int>(type: "int", nullable: false),
                    EquipoId = table.Column<int>(type: "int", nullable: false),
                    PlantillaChecklistId = table.Column<int>(type: "int", nullable: false),
                    ResultadoGeneral = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    ObservacionesEquipo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FechaRevision = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistTecnicoEquipos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChecklistTecnicoEquipos_ChecklistTecnicos_ChecklistTecnicoId",
                        column: x => x.ChecklistTecnicoId,
                        principalTable: "ChecklistTecnicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChecklistTecnicoEquipos_Equipos_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChecklistTecnicoEquipos_PlantillasChecklist_PlantillaChecklistId",
                        column: x => x.PlantillaChecklistId,
                        principalTable: "PlantillasChecklist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlantillaChecklistItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantillaChecklistId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    EsObligatorio = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantillaChecklistItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantillaChecklistItems_PlantillasChecklist_PlantillaChecklistId",
                        column: x => x.PlantillaChecklistId,
                        principalTable: "PlantillasChecklist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistTecnicoDetalles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChecklistTecnicoEquipoId = table.Column<int>(type: "int", nullable: false),
                    PlantillaChecklistItemId = table.Column<int>(type: "int", nullable: false),
                    EstadoRevision = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistTecnicoDetalles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChecklistTecnicoDetalles_ChecklistTecnicoEquipos_ChecklistTecnicoEquipoId",
                        column: x => x.ChecklistTecnicoEquipoId,
                        principalTable: "ChecklistTecnicoEquipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChecklistTecnicoDetalles_PlantillaChecklistItems_PlantillaChecklistItemId",
                        column: x => x.PlantillaChecklistItemId,
                        principalTable: "PlantillaChecklistItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistTecnicoDetalles_ChecklistTecnicoEquipoId",
                table: "ChecklistTecnicoDetalles",
                column: "ChecklistTecnicoEquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistTecnicoDetalles_PlantillaChecklistItemId",
                table: "ChecklistTecnicoDetalles",
                column: "PlantillaChecklistItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistTecnicoEquipos_ChecklistTecnicoId",
                table: "ChecklistTecnicoEquipos",
                column: "ChecklistTecnicoId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistTecnicoEquipos_EquipoId",
                table: "ChecklistTecnicoEquipos",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistTecnicoEquipos_PlantillaChecklistId",
                table: "ChecklistTecnicoEquipos",
                column: "PlantillaChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistTecnicos_TecnicoId",
                table: "ChecklistTecnicos",
                column: "TecnicoId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistTecnicos_UbicacionId",
                table: "ChecklistTecnicos",
                column: "UbicacionId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantillaChecklistItems_PlantillaChecklistId",
                table: "PlantillaChecklistItems",
                column: "PlantillaChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantillasChecklist_CategoriaEquipoId",
                table: "PlantillasChecklist",
                column: "CategoriaEquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantillasChecklist_TipoEquipoId",
                table: "PlantillasChecklist",
                column: "TipoEquipoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChecklistTecnicoDetalles");

            migrationBuilder.DropTable(
                name: "ChecklistTecnicoEquipos");

            migrationBuilder.DropTable(
                name: "PlantillaChecklistItems");

            migrationBuilder.DropTable(
                name: "ChecklistTecnicos");

            migrationBuilder.DropTable(
                name: "PlantillasChecklist");
        }
    }
}
