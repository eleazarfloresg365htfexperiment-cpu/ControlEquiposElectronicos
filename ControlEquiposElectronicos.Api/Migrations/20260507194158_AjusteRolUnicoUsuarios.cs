using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlEquiposElectronicos.Api.Migrations
{
    /// <inheritdoc />
    public partial class AjusteRolUnicoUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolPermisos_Permisos_PermisoId",
                table: "RolPermisos");

            migrationBuilder.DropForeignKey(
                name: "FK_RolPermisos_Roles_RolId",
                table: "RolPermisos");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Roles_RolId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_RolPermisos_RolId",
                table: "RolPermisos");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Usuarios",
                newName: "UsuarioId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Roles",
                newName: "RolId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ReclasificacionesEquipo",
                newName: "ReclasificacionEquipoId");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Usuarios",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreUsuario",
                table: "Usuarios",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "NombreRol",
                table: "Roles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EsSistema",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TipoReclasificacion",
                table: "ReclasificacionesEquipo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UbicacionAnteriorId",
                table: "ReclasificacionesEquipo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UbicacionNuevaId",
                table: "ReclasificacionesEquipo",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Modulo",
                table: "Permisos",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Accion",
                table: "Permisos",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_NombreUsuario",
                table: "Usuarios",
                column: "NombreUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolPermisos_RolId_PermisoId",
                table: "RolPermisos",
                columns: new[] { "RolId", "PermisoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_NombreRol",
                table: "Roles",
                column: "NombreRol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReclasificacionesEquipo_UbicacionAnteriorId",
                table: "ReclasificacionesEquipo",
                column: "UbicacionAnteriorId");

            migrationBuilder.CreateIndex(
                name: "IX_ReclasificacionesEquipo_UbicacionNuevaId",
                table: "ReclasificacionesEquipo",
                column: "UbicacionNuevaId");

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_Modulo_Accion",
                table: "Permisos",
                columns: new[] { "Modulo", "Accion" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReclasificacionesEquipo_Ubicaciones_UbicacionAnteriorId",
                table: "ReclasificacionesEquipo",
                column: "UbicacionAnteriorId",
                principalTable: "Ubicaciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReclasificacionesEquipo_Ubicaciones_UbicacionNuevaId",
                table: "ReclasificacionesEquipo",
                column: "UbicacionNuevaId",
                principalTable: "Ubicaciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RolPermisos_Permisos_PermisoId",
                table: "RolPermisos",
                column: "PermisoId",
                principalTable: "Permisos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RolPermisos_Roles_RolId",
                table: "RolPermisos",
                column: "RolId",
                principalTable: "Roles",
                principalColumn: "RolId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Roles_RolId",
                table: "Usuarios",
                column: "RolId",
                principalTable: "Roles",
                principalColumn: "RolId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReclasificacionesEquipo_Ubicaciones_UbicacionAnteriorId",
                table: "ReclasificacionesEquipo");

            migrationBuilder.DropForeignKey(
                name: "FK_ReclasificacionesEquipo_Ubicaciones_UbicacionNuevaId",
                table: "ReclasificacionesEquipo");

            migrationBuilder.DropForeignKey(
                name: "FK_RolPermisos_Permisos_PermisoId",
                table: "RolPermisos");

            migrationBuilder.DropForeignKey(
                name: "FK_RolPermisos_Roles_RolId",
                table: "RolPermisos");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Roles_RolId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_NombreUsuario",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_RolPermisos_RolId_PermisoId",
                table: "RolPermisos");

            migrationBuilder.DropIndex(
                name: "IX_Roles_NombreRol",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_ReclasificacionesEquipo_UbicacionAnteriorId",
                table: "ReclasificacionesEquipo");

            migrationBuilder.DropIndex(
                name: "IX_ReclasificacionesEquipo_UbicacionNuevaId",
                table: "ReclasificacionesEquipo");

            migrationBuilder.DropIndex(
                name: "IX_Permisos_Modulo_Accion",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "NombreUsuario",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "EsSistema",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "TipoReclasificacion",
                table: "ReclasificacionesEquipo");

            migrationBuilder.DropColumn(
                name: "UbicacionAnteriorId",
                table: "ReclasificacionesEquipo");

            migrationBuilder.DropColumn(
                name: "UbicacionNuevaId",
                table: "ReclasificacionesEquipo");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Usuarios",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "RolId",
                table: "Roles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ReclasificacionEquipoId",
                table: "ReclasificacionesEquipo",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "NombreRol",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Modulo",
                table: "Permisos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Accion",
                table: "Permisos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_RolPermisos_RolId",
                table: "RolPermisos",
                column: "RolId");

            migrationBuilder.AddForeignKey(
                name: "FK_RolPermisos_Permisos_PermisoId",
                table: "RolPermisos",
                column: "PermisoId",
                principalTable: "Permisos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolPermisos_Roles_RolId",
                table: "RolPermisos",
                column: "RolId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Roles_RolId",
                table: "Usuarios",
                column: "RolId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
