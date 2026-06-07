using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobApplicationTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddJAShortcutView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW View_ShortcutJA AS
                SELECT 
                    ja.Id AS jaId,
                    ja.UserId,
                    stat.Id AS statusId,
                    ev.Id AS eventId
                FROM 
                    JobApplications ja
                INNER JOIN JAStatusEntries stat ON ja.Id = stat.JobApplicationId
                LEFT JOIN JAEventEntries ev ON stat.Id = ev.JAStatusEntryId
                WHERE stat.Id = (
                    SELECT TOP 1 Id 
                    FROM JAStatusEntries 
                    WHERE JobApplicationId = ja.Id 
                    ORDER BY OrderIndex DESC
                )
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS View_MinimalJA");
        }
    }
}
