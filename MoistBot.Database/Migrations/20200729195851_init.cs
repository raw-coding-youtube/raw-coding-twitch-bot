using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MoistBot.Database.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TwitchMetadata",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Followed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchMetadata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TwitchMetadata_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TwitchSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(nullable: true),
                    TwitchUsername = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false),
                    SubscriptionPlan = table.Column<int>(nullable: false),
                    SubscriptionPlanName = table.Column<string>(nullable: true),
                    TotalMonths = table.Column<int>(nullable: false),
                    StreakMonths = table.Column<int>(nullable: false),
                    Context = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TwitchSubscriptions_TwitchMetadata_UserId",
                        column: x => x.UserId,
                        principalTable: "TwitchMetadata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TwitchMetadata_UserId",
                table: "TwitchMetadata",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TwitchSubscriptions_UserId",
                table: "TwitchSubscriptions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TwitchSubscriptions");

            migrationBuilder.DropTable(
                name: "TwitchMetadata");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
