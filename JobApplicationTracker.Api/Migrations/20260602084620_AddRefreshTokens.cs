using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobApplicationTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Education_ResumeEntries_UserResumeId",
                table: "Education");

            migrationBuilder.DropForeignKey(
                name: "FK_JAEventEntries_JAStatusEntries_JAStatusEntryId",
                table: "JAEventEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Users_UserId",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_JobListing_JobApplications_JobApplicationId",
                table: "JobListing");

            migrationBuilder.DropForeignKey(
                name: "FK_OtherExperience_ResumeEntries_UserResumeId",
                table: "OtherExperience");

            migrationBuilder.DropForeignKey(
                name: "FK_ResumeEntries_Users_UserId",
                table: "ResumeEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_ResumeSkill_ResumeEntries_UserResumeId",
                table: "ResumeSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillUsage_ResumeSkill_SkillId",
                table: "SkillUsage");

            migrationBuilder.DropForeignKey(
                name: "FK_Training_ResumeEntries_UserResumeId",
                table: "Training");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkExperience_ResumeEntries_UserResumeId",
                table: "WorkExperience");

            migrationBuilder.DropTable(
                name: "Experience");

            migrationBuilder.DropIndex(
                name: "IX_Users_NormalizedUserName",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_SkillUsage_ExperienceId",
                table: "SkillUsage");

            migrationBuilder.DropIndex(
                name: "IX_SkillUsage_SkillId",
                table: "SkillUsage");

            migrationBuilder.DropIndex(
                name: "IX_ResumeEntries_UserId",
                table: "ResumeEntries");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_UserId",
                table: "JobApplications");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EducationId",
                table: "SkillUsage",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OtherExperienceId",
                table: "SkillUsage",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ResumeSkillId",
                table: "SkillUsage",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TrainingId",
                table: "SkillUsage",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkExperienceId",
                table: "SkillUsage",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ResumeEntries",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "JaMinimalView",
                columns: table => new
                {
                    jaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JAStatus = table.Column<int>(type: "int", nullable: false),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsWholeDay = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkillUsage_EducationId",
                table: "SkillUsage",
                column: "EducationId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillUsage_OtherExperienceId",
                table: "SkillUsage",
                column: "OtherExperienceId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillUsage_ResumeSkillId",
                table: "SkillUsage",
                column: "ResumeSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillUsage_TrainingId",
                table: "SkillUsage",
                column: "TrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillUsage_WorkExperienceId",
                table: "SkillUsage",
                column: "WorkExperienceId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeEntries_UserId",
                table: "ResumeEntries",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Education_ResumeEntries_UserResumeId",
                table: "Education",
                column: "UserResumeId",
                principalTable: "ResumeEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JAEventEntries_JAStatusEntries_JAStatusEntryId",
                table: "JAEventEntries",
                column: "JAStatusEntryId",
                principalTable: "JAStatusEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobListing_JobApplications_JobApplicationId",
                table: "JobListing",
                column: "JobApplicationId",
                principalTable: "JobApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OtherExperience_ResumeEntries_UserResumeId",
                table: "OtherExperience",
                column: "UserResumeId",
                principalTable: "ResumeEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResumeEntries_Users_UserId",
                table: "ResumeEntries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResumeSkill_ResumeEntries_UserResumeId",
                table: "ResumeSkill",
                column: "UserResumeId",
                principalTable: "ResumeEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillUsage_Education_EducationId",
                table: "SkillUsage",
                column: "EducationId",
                principalTable: "Education",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SkillUsage_OtherExperience_OtherExperienceId",
                table: "SkillUsage",
                column: "OtherExperienceId",
                principalTable: "OtherExperience",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SkillUsage_ResumeSkill_ResumeSkillId",
                table: "SkillUsage",
                column: "ResumeSkillId",
                principalTable: "ResumeSkill",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SkillUsage_Training_TrainingId",
                table: "SkillUsage",
                column: "TrainingId",
                principalTable: "Training",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SkillUsage_WorkExperience_WorkExperienceId",
                table: "SkillUsage",
                column: "WorkExperienceId",
                principalTable: "WorkExperience",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Training_ResumeEntries_UserResumeId",
                table: "Training",
                column: "UserResumeId",
                principalTable: "ResumeEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkExperience_ResumeEntries_UserResumeId",
                table: "WorkExperience",
                column: "UserResumeId",
                principalTable: "ResumeEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Education_ResumeEntries_UserResumeId",
                table: "Education");

            migrationBuilder.DropForeignKey(
                name: "FK_JAEventEntries_JAStatusEntries_JAStatusEntryId",
                table: "JAEventEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_JobListing_JobApplications_JobApplicationId",
                table: "JobListing");

            migrationBuilder.DropForeignKey(
                name: "FK_OtherExperience_ResumeEntries_UserResumeId",
                table: "OtherExperience");

            migrationBuilder.DropForeignKey(
                name: "FK_ResumeEntries_Users_UserId",
                table: "ResumeEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_ResumeSkill_ResumeEntries_UserResumeId",
                table: "ResumeSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillUsage_Education_EducationId",
                table: "SkillUsage");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillUsage_OtherExperience_OtherExperienceId",
                table: "SkillUsage");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillUsage_ResumeSkill_ResumeSkillId",
                table: "SkillUsage");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillUsage_Training_TrainingId",
                table: "SkillUsage");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillUsage_WorkExperience_WorkExperienceId",
                table: "SkillUsage");

            migrationBuilder.DropForeignKey(
                name: "FK_Training_ResumeEntries_UserResumeId",
                table: "Training");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkExperience_ResumeEntries_UserResumeId",
                table: "WorkExperience");

            migrationBuilder.DropTable(
                name: "JaMinimalView");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_SkillUsage_EducationId",
                table: "SkillUsage");

            migrationBuilder.DropIndex(
                name: "IX_SkillUsage_OtherExperienceId",
                table: "SkillUsage");

            migrationBuilder.DropIndex(
                name: "IX_SkillUsage_ResumeSkillId",
                table: "SkillUsage");

            migrationBuilder.DropIndex(
                name: "IX_SkillUsage_TrainingId",
                table: "SkillUsage");

            migrationBuilder.DropIndex(
                name: "IX_SkillUsage_WorkExperienceId",
                table: "SkillUsage");

            migrationBuilder.DropIndex(
                name: "IX_ResumeEntries_UserId",
                table: "ResumeEntries");

            migrationBuilder.DropColumn(
                name: "EducationId",
                table: "SkillUsage");

            migrationBuilder.DropColumn(
                name: "OtherExperienceId",
                table: "SkillUsage");

            migrationBuilder.DropColumn(
                name: "ResumeSkillId",
                table: "SkillUsage");

            migrationBuilder.DropColumn(
                name: "TrainingId",
                table: "SkillUsage");

            migrationBuilder.DropColumn(
                name: "WorkExperienceId",
                table: "SkillUsage");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "Users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "Users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ResumeEntries",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "JobApplications",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Experience",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserResumeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experience", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_NormalizedUserName",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SkillUsage_ExperienceId",
                table: "SkillUsage",
                column: "ExperienceId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillUsage_SkillId",
                table: "SkillUsage",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeEntries_UserId",
                table: "ResumeEntries",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_UserId",
                table: "JobApplications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Education_ResumeEntries_UserResumeId",
                table: "Education",
                column: "UserResumeId",
                principalTable: "ResumeEntries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JAEventEntries_JAStatusEntries_JAStatusEntryId",
                table: "JAEventEntries",
                column: "JAStatusEntryId",
                principalTable: "JAStatusEntries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Users_UserId",
                table: "JobApplications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobListing_JobApplications_JobApplicationId",
                table: "JobListing",
                column: "JobApplicationId",
                principalTable: "JobApplications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OtherExperience_ResumeEntries_UserResumeId",
                table: "OtherExperience",
                column: "UserResumeId",
                principalTable: "ResumeEntries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ResumeEntries_Users_UserId",
                table: "ResumeEntries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ResumeSkill_ResumeEntries_UserResumeId",
                table: "ResumeSkill",
                column: "UserResumeId",
                principalTable: "ResumeEntries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SkillUsage_ResumeSkill_SkillId",
                table: "SkillUsage",
                column: "SkillId",
                principalTable: "ResumeSkill",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Training_ResumeEntries_UserResumeId",
                table: "Training",
                column: "UserResumeId",
                principalTable: "ResumeEntries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkExperience_ResumeEntries_UserResumeId",
                table: "WorkExperience",
                column: "UserResumeId",
                principalTable: "ResumeEntries",
                principalColumn: "Id");
        }
    }
}
