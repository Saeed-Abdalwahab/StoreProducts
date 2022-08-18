using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Store_TechniaclTask.DAL.Migrations
{
    public partial class addremindertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoppingStoreReminder",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShoppingStoreID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    IsSent = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingStoreReminder", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ShoppingStoreReminder_ShoppingStores_ShoppingStoreID",
                        column: x => x.ShoppingStoreID,
                        principalTable: "ShoppingStores",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingStoreReminder_ShoppingStoreID",
                table: "ShoppingStoreReminder",
                column: "ShoppingStoreID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingStoreReminder");
        }
    }
}
