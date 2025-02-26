using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationCore.Migrations.Doc
{
    /// <inheritdoc />
    public partial class post_reader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Posts_ReaderId",
                table: "Posts",
                column: "ReaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Readers_ReaderId",
                table: "Posts",
                column: "ReaderId",
                principalTable: "Readers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Readers_ReaderId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_ReaderId",
                table: "Posts");
        }
    }
}
