using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestKuy.Migrations
{
    public partial class OrderShippingCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    OrderTotal = table.Column<float>(nullable: false),
                    OrderPlaced = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shippings",
                columns: table => new
                {
                    ShipName = table.Column<string>(nullable: false),
                    ShipPhone = table.Column<int>(nullable: false),
                    ShipCountry = table.Column<string>(nullable: true),
                    ShipState = table.Column<string>(nullable: true),
                    ShipZipcode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shippings", x => x.ShipName);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    imageProductId = table.Column<string>(nullable: true),
                    shippingShipName = table.Column<string>(nullable: true),
                    Quantity = table.Column<float>(nullable: false),
                    CartId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Images_imageProductId",
                        column: x => x.imageProductId,
                        principalTable: "Images",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CartItems_Shippings_shippingShipName",
                        column: x => x.shippingShipName,
                        principalTable: "Shippings",
                        principalColumn: "ShipName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Quantity = table.Column<float>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    OrderId = table.Column<string>(nullable: true),
                    VGAId = table.Column<string>(nullable: true),
                    VGAimage = table.Column<string>(nullable: true),
                    VGAName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Phone = table.Column<int>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Zipcode = table.Column<int>(nullable: false),
                    imageProductId = table.Column<string>(nullable: true),
                    shippingShipName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Images_imageProductId",
                        column: x => x.imageProductId,
                        principalTable: "Images",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Shippings_shippingShipName",
                        column: x => x.shippingShipName,
                        principalTable: "Shippings",
                        principalColumn: "ShipName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_imageProductId",
                table: "CartItems",
                column: "imageProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_shippingShipName",
                table: "CartItems",
                column: "shippingShipName");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_imageProductId",
                table: "OrderItems",
                column: "imageProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_shippingShipName",
                table: "OrderItems",
                column: "shippingShipName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Shippings");
        }
    }
}
