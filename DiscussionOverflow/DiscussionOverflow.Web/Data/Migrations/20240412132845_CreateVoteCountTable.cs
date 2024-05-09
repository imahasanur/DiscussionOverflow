using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscussionOverflow.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateVoteCountTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vote",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    QuestionMaker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Replier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpVote = table.Column<int>(type: "int", nullable: true),
                    DownVote = table.Column<int>(type: "int", nullable: true),
                    Voter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vote_Answer_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vote_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Answer",
                keyColumn: "Id",
                keyValue: new Guid("68503176-0314-4937-92e4-400a6f4f4472"),
                column: "TimeStamp",
                value: new DateTime(2024, 4, 12, 17, 36, 39, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Answer",
                keyColumn: "Id",
                keyValue: new Guid("963c81f0-4bf3-4c91-961d-945c2d8872f8"),
                column: "TimeStamp",
                value: new DateTime(2024, 4, 12, 17, 36, 39, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Comment",
                keyColumn: "Id",
                keyValue: new Guid("69a75666-c25d-4736-92f6-66bbbc08d926"),
                column: "TimeStamp",
                value: new DateTime(2024, 4, 12, 17, 36, 39, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Comment",
                keyColumn: "Id",
                keyValue: new Guid("cd19778a-fb86-4ac9-96a4-cd4c5b423018"),
                column: "TimeStamp",
                value: new DateTime(2024, 4, 12, 17, 36, 39, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Question",
                keyColumn: "Id",
                keyValue: new Guid("003805c3-938c-43b7-a768-03d6c0242ece"),
                column: "TimeStamp",
                value: new DateTime(2024, 4, 12, 17, 36, 39, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Question",
                keyColumn: "Id",
                keyValue: new Guid("e580cf4d-fa7a-421d-8d0f-ab406d0a2e23"),
                column: "TimeStamp",
                value: new DateTime(2024, 4, 12, 17, 36, 39, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Vote_AnswerId",
                table: "Vote",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_QuestionId",
                table: "Vote",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vote");

            migrationBuilder.UpdateData(
                table: "Answer",
                keyColumn: "Id",
                keyValue: new Guid("68503176-0314-4937-92e4-400a6f4f4472"),
                column: "TimeStamp",
                value: new DateTime(2024, 4, 12, 17, 36, 39, 455, DateTimeKind.Local).AddTicks(3896));

            migrationBuilder.UpdateData(
                table: "Answer",
                keyColumn: "Id",
                keyValue: new Guid("963c81f0-4bf3-4c91-961d-945c2d8872f8"),
                column: "TimeStamp",
                value: new DateTime(2024, 4, 12, 17, 36, 39, 455, DateTimeKind.Local).AddTicks(3889));

            migrationBuilder.UpdateData(
                table: "Comment",
                keyColumn: "Id",
                keyValue: new Guid("69a75666-c25d-4736-92f6-66bbbc08d926"),
                column: "TimeStamp",
                value: new DateTime(2024, 4, 12, 17, 36, 39, 455, DateTimeKind.Local).AddTicks(3943));

            migrationBuilder.UpdateData(
                table: "Comment",
                keyColumn: "Id",
                keyValue: new Guid("cd19778a-fb86-4ac9-96a4-cd4c5b423018"),
                column: "TimeStamp",
                value: new DateTime(2024, 4, 12, 17, 36, 39, 455, DateTimeKind.Local).AddTicks(3954));

            migrationBuilder.UpdateData(
                table: "Question",
                keyColumn: "Id",
                keyValue: new Guid("003805c3-938c-43b7-a768-03d6c0242ece"),
                column: "TimeStamp",
                value: new DateTime(2024, 4, 12, 17, 36, 39, 455, DateTimeKind.Local).AddTicks(3165));

            migrationBuilder.UpdateData(
                table: "Question",
                keyColumn: "Id",
                keyValue: new Guid("e580cf4d-fa7a-421d-8d0f-ab406d0a2e23"),
                column: "TimeStamp",
                value: new DateTime(2024, 4, 12, 17, 36, 39, 455, DateTimeKind.Local).AddTicks(3193));
        }
    }
}
