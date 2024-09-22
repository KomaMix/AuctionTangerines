using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionTangerines.Migrations
{
    /// <inheritdoc />
    public partial class AddTangerineAndBet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tangerines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CostBuyout = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TimeBuyout = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserBuyoutId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tangerines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tangerines_AspNetUsers_UserBuyoutId",
                        column: x => x.UserBuyoutId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Bets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TangerineId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bets_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bets_Tangerines_TangerineId",
                        column: x => x.TangerineId,
                        principalTable: "Tangerines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bets_AppUserId",
                table: "Bets",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Bets_TangerineId",
                table: "Bets",
                column: "TangerineId");

            migrationBuilder.CreateIndex(
                name: "IX_Tangerines_UserBuyoutId",
                table: "Tangerines",
                column: "UserBuyoutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bets");

            migrationBuilder.DropTable(
                name: "Tangerines");
        }
    }
}
