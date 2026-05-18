using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobApplicationTracker.Migrations
{
    /// <inheritdoc />
    public partial class FixTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experience_ResumeEntries_UserResumeId",
                table: "Experience");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillUsage_Experience_ExperienceId",
                table: "SkillUsage");

            migrationBuilder.DropIndex(
                name: "IX_Experience_UserResumeId",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "Certification",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "Degree",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "ExperienceType",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "JobDescription",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "Major",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "OtherExperience_Notes",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "School",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "Training_Notes",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "WorkExperience_EndDate",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "WorkExperience_Notes",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "WorkExperience_StartDate",
                table: "Experience");

            migrationBuilder.CreateTable(
                name: "Education",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserResumeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Degree = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    School = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Major = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Education", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Education_ResumeEntries_UserResumeId",
                        column: x => x.UserResumeId,
                        principalTable: "ResumeEntries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OtherExperience",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserResumeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherExperience", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtherExperience_ResumeEntries_UserResumeId",
                        column: x => x.UserResumeId,
                        principalTable: "ResumeEntries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Training",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserResumeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Certification = table.Column<string>(type: "nvarchar(2500)", maxLength: 2500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2500)", maxLength: 2500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Training", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Training_ResumeEntries_UserResumeId",
                        column: x => x.UserResumeId,
                        principalTable: "ResumeEntries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkExperience",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserResumeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    JobDescription = table.Column<string>(type: "nvarchar(2500)", maxLength: 2500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2500)", maxLength: 2500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkExperience", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkExperience_ResumeEntries_UserResumeId",
                        column: x => x.UserResumeId,
                        principalTable: "ResumeEntries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Education_UserResumeId",
                table: "Education",
                column: "UserResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_OtherExperience_UserResumeId",
                table: "OtherExperience",
                column: "UserResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_Training_UserResumeId",
                table: "Training",
                column: "UserResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperience_UserResumeId",
                table: "WorkExperience",
                column: "UserResumeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Education");

            migrationBuilder.DropTable(
                name: "OtherExperience");

            migrationBuilder.DropTable(
                name: "Training");

            migrationBuilder.DropTable(
                name: "WorkExperience");

            migrationBuilder.AddColumn<string>(
                name: "Certification",
                table: "Experience",
                type: "nvarchar(2500)",
                maxLength: 2500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Experience",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Degree",
                table: "Experience",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "Experience",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExperienceType",
                table: "Experience",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "Experience",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobDescription",
                table: "Experience",
                type: "nvarchar(2500)",
                maxLength: 2500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Major",
                table: "Experience",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Experience",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Experience",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherExperience_Notes",
                table: "Experience",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Experience",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "School",
                table: "Experience",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "Experience",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Training_Notes",
                table: "Experience",
                type: "nvarchar(2500)",
                maxLength: 2500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Experience",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "WorkExperience_EndDate",
                table: "Experience",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkExperience_Notes",
                table: "Experience",
                type: "nvarchar(2500)",
                maxLength: 2500,
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "WorkExperience_StartDate",
                table: "Experience",
                type: "date",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Experience_UserResumeId",
                table: "Experience",
                column: "UserResumeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Experience_ResumeEntries_UserResumeId",
                table: "Experience",
                column: "UserResumeId",
                principalTable: "ResumeEntries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SkillUsage_Experience_ExperienceId",
                table: "SkillUsage",
                column: "ExperienceId",
                principalTable: "Experience",
                principalColumn: "Id");
        }
    }
}
