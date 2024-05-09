using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DiscussionOverflow.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataOfVoteCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Vote",
                columns: new[] { "Id", "AnswerId", "DownVote", "QuestionId", "QuestionMaker", "Replier", "TimeStamp", "UpVote", "Voter" },
                values: new object[,]
                {
                    { new Guid("20ed9bee-e628-4c44-ba3d-b25a68827dcc"), new Guid("963c81f0-4bf3-4c91-961d-945c2d8872f8"), 1, null, null, "skill1@gmail.com", new DateTime(2024, 4, 12, 18, 36, 39, 0, DateTimeKind.Unspecified), null, "skill2@gmail.com" },
                    { new Guid("a58e02ef-a025-492c-9746-1db9d2067d36"), null, null, new Guid("003805c3-938c-43b7-a768-03d6c0242ece"), "skill1@gmail.com", null, new DateTime(2024, 4, 12, 18, 36, 39, 0, DateTimeKind.Unspecified), 1, "skill2@gmail.com" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vote",
                keyColumn: "Id",
                keyValue: new Guid("20ed9bee-e628-4c44-ba3d-b25a68827dcc"));

            migrationBuilder.DeleteData(
                table: "Vote",
                keyColumn: "Id",
                keyValue: new Guid("a58e02ef-a025-492c-9746-1db9d2067d36"));
        }
    }
}
