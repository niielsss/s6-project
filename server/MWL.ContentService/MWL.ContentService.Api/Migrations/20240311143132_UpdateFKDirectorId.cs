using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MWL.ContentService.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFKDirectorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_People_DirectorId",
                table: "Movies");

            migrationBuilder.AlterColumn<int>(
                name: "DirectorId",
                table: "Movies",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_People_DirectorId",
                table: "Movies",
                column: "DirectorId",
                principalTable: "People",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_People_DirectorId",
                table: "Movies");

            migrationBuilder.AlterColumn<int>(
                name: "DirectorId",
                table: "Movies",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_People_DirectorId",
                table: "Movies",
                column: "DirectorId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
