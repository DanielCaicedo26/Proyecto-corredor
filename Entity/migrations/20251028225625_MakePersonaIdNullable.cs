using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class MakePersonaIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Personas_PersonaId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "PersonaId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PermissionId1",
                table: "RoleFormPermissions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleFormPermissions_PermissionId1",
                table: "RoleFormPermissions",
                column: "PermissionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleFormPermissions_Permissions_PermissionId1",
                table: "RoleFormPermissions",
                column: "PermissionId1",
                principalTable: "Permissions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Personas_PersonaId",
                table: "Users",
                column: "PersonaId",
                principalTable: "Personas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleFormPermissions_Permissions_PermissionId1",
                table: "RoleFormPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Personas_PersonaId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_RoleFormPermissions_PermissionId1",
                table: "RoleFormPermissions");

            migrationBuilder.DropColumn(
                name: "PermissionId1",
                table: "RoleFormPermissions");

            migrationBuilder.AlterColumn<int>(
                name: "PersonaId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Personas_PersonaId",
                table: "Users",
                column: "PersonaId",
                principalTable: "Personas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
