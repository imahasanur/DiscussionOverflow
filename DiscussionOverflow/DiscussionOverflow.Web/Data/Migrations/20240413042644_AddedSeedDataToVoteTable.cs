using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DiscussionOverflow.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeedDataToVoteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Vote",
                columns: new[] { "Id", "AnswerId", "DownVote", "QuestionId", "QuestionMaker", "Replier", "TimeStamp", "UpVote", "Voter" },
                values: new object[,]
                {
                    { new Guid("6559b465-83ae-4005-a415-2e24d2728cc4"), null, 1, new Guid("e580cf4d-fa7a-421d-8d0f-ab406d0a2e23"), "skill1@gmail.com", null, new DateTime(2024, 4, 12, 18, 36, 39, 0, DateTimeKind.Unspecified), 1, "skill3@gmail.com" },
                    { new Guid("6aaf6885-7b65-44a0-9e65-9b89c28f7673"), new Guid("68503176-0314-4937-92e4-400a6f4f4472"), 1, null, null, "skill2@gmail.com", new DateTime(2024, 4, 12, 18, 36, 39, 0, DateTimeKind.Unspecified), 1, "skill3@gmail.com" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vote",
                keyColumn: "Id",
                keyValue: new Guid("6559b465-83ae-4005-a415-2e24d2728cc4"));

            migrationBuilder.DeleteData(
                table: "Vote",
                keyColumn: "Id",
                keyValue: new Guid("6aaf6885-7b65-44a0-9e65-9b89c28f7673"));
        }
    }
}
