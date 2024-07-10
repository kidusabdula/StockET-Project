using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class CommentOneToOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1887f13a-410b-476d-9dad-42f93ec85a39");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "283ddb34-8044-42db-86a6-98bcc2e85602");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "279510a8-3dad-4c27-bfa6-ba71357def16", null, "Admin", "ADMIN" },
                    { "3a204e2d-a036-4fb4-9556-4d2123f6a5ad", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "279510a8-3dad-4c27-bfa6-ba71357def16");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3a204e2d-a036-4fb4-9556-4d2123f6a5ad");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1887f13a-410b-476d-9dad-42f93ec85a39", null, "User", "USER" },
                    { "283ddb34-8044-42db-86a6-98bcc2e85602", null, "Admin", "ADMIN" }
                });
        }
    }
}
