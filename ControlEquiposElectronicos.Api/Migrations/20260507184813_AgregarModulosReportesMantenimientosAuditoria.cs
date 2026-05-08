using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlEquiposElectronicos.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarModulosReportesMantenimientosAuditoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstadosReporte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosReporte", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HistorialEstadosEquipo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipoId = table.Column<int>(type: "int", nullable: false),
                    EstadoAnteriorId = table.Column<int>(type: "int", nullable: false),
                    EstadoNuevoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCambio = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialEstadosEquipo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialEstadosEquipo_Equipos_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistorialEstadosEquipo_EstadosEquipo_EstadoAnteriorId",
                        column: x => x.EstadoAnteriorId,
                        principalTable: "EstadosEquipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistorialEstadosEquipo_EstadosEquipo_EstadoNuevoId",
                        column: x => x.EstadoNuevoId,
                        principalTable: "EstadosEquipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistorialEstadosEquipo_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistorialOperaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    Accion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TablaAfectada = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistroId = table.Column<int>(type: "int", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DireccionIP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaOperacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialOperaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialOperaciones_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReclasificacionesEquipo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    CodigoAnterior = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoNuevo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoAnteriorId = table.Column<int>(type: "int", nullable: true),
                    EstadoNuevoId = table.Column<int>(type: "int", nullable: true),
                    Motivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaReclasificacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReclasificacionesEquipo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReclasificacionesEquipo_Equipos_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReclasificacionesEquipo_EstadosEquipo_EstadoAnteriorId",
                        column: x => x.EstadoAnteriorId,
                        principalTable: "EstadosEquipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReclasificacionesEquipo_EstadosEquipo_EstadoNuevoId",
                        column: x => x.EstadoNuevoId,
                        principalTable: "EstadosEquipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReclasificacionesEquipo_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TiposMantenimiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposMantenimiento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportesFalla",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioReportaId = table.Column<int>(type: "int", nullable: false),
                    EstadoReporteId = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prioridad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaReporte = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaCierre = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ObservacionesCierre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportesFalla", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportesFalla_Equipos_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportesFalla_EstadosReporte_EstadoReporteId",
                        column: x => x.EstadoReporteId,
                        principalTable: "EstadosReporte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportesFalla_Usuarios_UsuarioReportaId",
                        column: x => x.UsuarioReportaId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mantenimientos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipoId = table.Column<int>(type: "int", nullable: false),
                    ReporteFallaId = table.Column<int>(type: "int", nullable: true),
                    TipoMantenimientoId = table.Column<int>(type: "int", nullable: false),
                    TecnicoId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Diagnostico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Resultado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CostoEstimado = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    EstadoMantenimiento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mantenimientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mantenimientos_Equipos_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mantenimientos_ReportesFalla_ReporteFallaId",
                        column: x => x.ReporteFallaId,
                        principalTable: "ReportesFalla",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mantenimientos_TiposMantenimiento_TipoMantenimientoId",
                        column: x => x.TipoMantenimientoId,
                        principalTable: "TiposMantenimiento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mantenimientos_Usuarios_TecnicoId",
                        column: x => x.TecnicoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MantenimientoRepuestos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MantenimientoId = table.Column<int>(type: "int", nullable: false),
                    NombreRepuesto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    CostoUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    NumeroSerieAnterior = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroSerieNuevo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MantenimientoRepuestos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MantenimientoRepuestos_Mantenimientos_MantenimientoId",
                        column: x => x.MantenimientoId,
                        principalTable: "Mantenimientos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistorialEstadosEquipo_EquipoId",
                table: "HistorialEstadosEquipo",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialEstadosEquipo_EstadoAnteriorId",
                table: "HistorialEstadosEquipo",
                column: "EstadoAnteriorId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialEstadosEquipo_EstadoNuevoId",
                table: "HistorialEstadosEquipo",
                column: "EstadoNuevoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialEstadosEquipo_UsuarioId",
                table: "HistorialEstadosEquipo",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialOperaciones_UsuarioId",
                table: "HistorialOperaciones",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_MantenimientoRepuestos_MantenimientoId",
                table: "MantenimientoRepuestos",
                column: "MantenimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimientos_EquipoId",
                table: "Mantenimientos",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimientos_ReporteFallaId",
                table: "Mantenimientos",
                column: "ReporteFallaId");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimientos_TecnicoId",
                table: "Mantenimientos",
                column: "TecnicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimientos_TipoMantenimientoId",
                table: "Mantenimientos",
                column: "TipoMantenimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_ReclasificacionesEquipo_EquipoId",
                table: "ReclasificacionesEquipo",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_ReclasificacionesEquipo_EstadoAnteriorId",
                table: "ReclasificacionesEquipo",
                column: "EstadoAnteriorId");

            migrationBuilder.CreateIndex(
                name: "IX_ReclasificacionesEquipo_EstadoNuevoId",
                table: "ReclasificacionesEquipo",
                column: "EstadoNuevoId");

            migrationBuilder.CreateIndex(
                name: "IX_ReclasificacionesEquipo_UsuarioId",
                table: "ReclasificacionesEquipo",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportesFalla_EquipoId",
                table: "ReportesFalla",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportesFalla_EstadoReporteId",
                table: "ReportesFalla",
                column: "EstadoReporteId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportesFalla_UsuarioReportaId",
                table: "ReportesFalla",
                column: "UsuarioReportaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistorialEstadosEquipo");

            migrationBuilder.DropTable(
                name: "HistorialOperaciones");

            migrationBuilder.DropTable(
                name: "MantenimientoRepuestos");

            migrationBuilder.DropTable(
                name: "ReclasificacionesEquipo");

            migrationBuilder.DropTable(
                name: "Mantenimientos");

            migrationBuilder.DropTable(
                name: "ReportesFalla");

            migrationBuilder.DropTable(
                name: "TiposMantenimiento");

            migrationBuilder.DropTable(
                name: "EstadosReporte");
        }
    }
}
