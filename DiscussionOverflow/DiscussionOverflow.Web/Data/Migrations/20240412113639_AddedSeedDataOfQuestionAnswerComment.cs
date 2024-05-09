using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DiscussionOverflow.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeedDataOfQuestionAnswerComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Question",
                columns: new[] { "Id", "CurrentStatus", "Details", "QuestionMaker", "Tags", "TimeStamp", "Title" },
                values: new object[,]
                {
                    { new Guid("003805c3-938c-43b7-a768-03d6c0242ece"), "lorem impsum de color sutracun lorem impsum de color lorem ipsum de color", "lorem impsum de color sutracun lorem impsum de color lorem ipsum de color", "skill1@gmail.com", "C,F,D", new DateTime(2024, 4, 12, 17, 36, 39, 455, DateTimeKind.Local).AddTicks(3165), "This is the test Question 1" },
                    { new Guid("e580cf4d-fa7a-421d-8d0f-ab406d0a2e23"), "lorem impsum de color sutracun lorem impsum de color lorem ipsum de color", "lorem impsum de color sutracun lorem impsum de color lorem ipsum de color", "skill1@gmail.com", "C,F,D,E", new DateTime(2024, 4, 12, 17, 36, 39, 455, DateTimeKind.Local).AddTicks(3193), "This is the test Question 2" }
                });

            migrationBuilder.InsertData(
                table: "Answer",
                columns: new[] { "Id", "AnswerBody", "QuestionId", "QuestionMaker", "Replier", "TimeStamp" },
                values: new object[,]
                {
                    { new Guid("68503176-0314-4937-92e4-400a6f4f4472"), "lorem impsumlorem impsum lorem impsum lorem impsum", new Guid("003805c3-938c-43b7-a768-03d6c0242ece"), "skill1@gmail.com", "skill2@gmail.com", new DateTime(2024, 4, 12, 17, 36, 39, 455, DateTimeKind.Local).AddTicks(3896) },
                    { new Guid("963c81f0-4bf3-4c91-961d-945c2d8872f8"), "lorem impsumlorem impsum lorem impsum lorem impsum", new Guid("003805c3-938c-43b7-a768-03d6c0242ece"), "skill1@gmail.com", "skill1@gmail.com", new DateTime(2024, 4, 12, 17, 36, 39, 455, DateTimeKind.Local).AddTicks(3889) }
                });

            migrationBuilder.InsertData(
                table: "Comment",
                columns: new[] { "Id", "AnswerId", "CommentBody", "Commentator", "QuestionId", "QuestionMaker", "Replier", "TimeStamp" },
                values: new object[,]
                {
                    { new Guid("69a75666-c25d-4736-92f6-66bbbc08d926"), null, "lorem impsumlorem impsum lorem impsum lorem impsum", "skill2@gmail.com", new Guid("003805c3-938c-43b7-a768-03d6c0242ece"), "skill1@gmail.com", null, new DateTime(2024, 4, 12, 17, 36, 39, 455, DateTimeKind.Local).AddTicks(3943) },
                    { new Guid("cd19778a-fb86-4ac9-96a4-cd4c5b423018"), new Guid("963c81f0-4bf3-4c91-961d-945c2d8872f8"), "lorem impsum  lorem impsum lorem impsum lorem impsum ,,,,,,,,", "skill2@gmail.com", null, null, "skill1@gmail.com", new DateTime(2024, 4, 12, 17, 36, 39, 455, DateTimeKind.Local).AddTicks(3954) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Answer",
                keyColumn: "Id",
                keyValue: new Guid("68503176-0314-4937-92e4-400a6f4f4472"));

            migrationBuilder.DeleteData(
                table: "Comment",
                keyColumn: "Id",
                keyValue: new Guid("69a75666-c25d-4736-92f6-66bbbc08d926"));

            migrationBuilder.DeleteData(
                table: "Comment",
                keyColumn: "Id",
                keyValue: new Guid("cd19778a-fb86-4ac9-96a4-cd4c5b423018"));

            migrationBuilder.DeleteData(
                table: "Question",
                keyColumn: "Id",
                keyValue: new Guid("e580cf4d-fa7a-421d-8d0f-ab406d0a2e23"));

            migrationBuilder.DeleteData(
                table: "Answer",
                keyColumn: "Id",
                keyValue: new Guid("963c81f0-4bf3-4c91-961d-945c2d8872f8"));

            migrationBuilder.DeleteData(
                table: "Question",
                keyColumn: "Id",
                keyValue: new Guid("003805c3-938c-43b7-a768-03d6c0242ece"));
        }
    }
}
