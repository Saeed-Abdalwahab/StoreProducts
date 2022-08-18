using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Store_TechniaclTask.DAL.Migrations
{
    public partial class addstortable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoppingStores",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(nullable: true),
                    RegistrationDate = table.Column<DateTime>(nullable: false),
                    ShoppingStoreStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingStores", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ShoppingStores_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingStoreDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShoppingStoreID = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    ProductPrice = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingStoreDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ShoppingStoreDetails_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShoppingStoreDetails_ShoppingStores_ShoppingStoreID",
                        column: x => x.ShoppingStoreID,
                        principalTable: "ShoppingStores",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingStoreDetails_ShoppingStoreID",
                table: "ShoppingStoreDetails",
                column: "ShoppingStoreID");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingStoreDetails_ProductID_ShoppingStoreID",
                table: "ShoppingStoreDetails",
                columns: new[] { "ProductID", "ShoppingStoreID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingStores_UserID",
                table: "ShoppingStores",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingStoreDetails");

            migrationBuilder.DropTable(
                name: "ShoppingStores");
        }
    }
}
