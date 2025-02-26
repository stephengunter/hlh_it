using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationCore.Migrations.Doc
{
    /// <inheritdoc />
    public partial class unpost_reader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Readers_ReaderId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_ReaderId",
                table: "Posts");

            migrationBuilder.CreateTable(
                name: "PostReaders",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false),
                    ReaderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostReaders", x => new { x.PostId, x.ReaderId });
                    table.ForeignKey(
                        name: "FK_PostReaders_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostReaders_Readers_ReaderId",
                        column: x => x.ReaderId,
                        principalTable: "Readers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostReaders_ReaderId",
                table: "PostReaders",
                column: "ReaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostReaders");

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
    }
}
