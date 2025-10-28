using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class DropPersonaIdForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the existing FK constraint
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Personas_PersonaId",
                table: "Users");

            // Alter column to allow NULL
            migrationBuilder.AlterColumn<int>(
                name: "PersonaId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            // Re-create the FK with ON DELETE SET NULL
            migrationBuilder.AddForeignKey(
                name: "FK_Users_Personas_PersonaId",
                table: "Users",
                column: "PersonaId",
                principalTable: "Personas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the modified FK
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Personas_PersonaId",
                table: "Users");

            // Restore as NOT NULL
            migrationBuilder.AlterColumn<int>(
                name: "PersonaId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // Re-create original FK
            migrationBuilder.AddForeignKey(
                name: "FK_Users_Personas_PersonaId",
                table: "Users",
                column: "PersonaId",
                principalTable: "Personas",
                principalColumn: "Id");
        }
    }
}
