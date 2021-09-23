using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiTest.Migrations
{
    public partial class Availble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Wifes_WantedListId",
                table: "Wifes",
                column: "WantedListId");

            migrationBuilder.CreateIndex(
                name: "IX_Husbands_WifeId",
                table: "Husbands",
                column: "WifeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Husbands_Wifes_WifeId",
                table: "Husbands",
                column: "WifeId",
                principalTable: "Wifes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wifes_WantedLists_WantedListId",
                table: "Wifes",
                column: "WantedListId",
                principalTable: "WantedLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Husbands_Wifes_WifeId",
                table: "Husbands");

            migrationBuilder.DropForeignKey(
                name: "FK_Wifes_WantedLists_WantedListId",
                table: "Wifes");

            migrationBuilder.DropIndex(
                name: "IX_Wifes_WantedListId",
                table: "Wifes");

            migrationBuilder.DropIndex(
                name: "IX_Husbands_WifeId",
                table: "Husbands");
        }
    }
}
