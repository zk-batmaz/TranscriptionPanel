using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TranscriptionBackend.Migrations
{
  /// <inheritdoc />
  public partial class AddActivityTable : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_Activities_Users_UserId",
          table: "Activities");

      migrationBuilder.DropIndex(
          name: "IX_Activities_UserId",
          table: "Activities");

      migrationBuilder.AlterColumn<string>(
          name: "FileName",
          table: "Activities",
          type: "nvarchar(max)",
          nullable: false,
          defaultValue: "",
          oldClrType: typeof(string),
          oldType: "nvarchar(max)",
          oldNullable: true);

      migrationBuilder.AddColumn<string>(
          name: "ActivityMessage",
          table: "Activities",
          type: "nvarchar(max)",
          nullable: false,
          defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "ActivityMessage",
          table: "Activities");

      migrationBuilder.AlterColumn<string>(
          name: "FileName",
          table: "Activities",
          type: "nvarchar(max)",
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(max)");

      migrationBuilder.CreateIndex(
          name: "IX_Activities_UserId",
          table: "Activities",
          column: "UserId");

      migrationBuilder.AddForeignKey(
          name: "FK_Activities_Users_UserId",
          table: "Activities",
          column: "UserId",
          principalTable: "Users",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);
    }
  }
}
