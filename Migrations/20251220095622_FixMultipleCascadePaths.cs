using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceSystemMVC.Migrations
{
    /// <inheritdoc />
    public partial class FixMultipleCascadePaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRecords_Sections_SectionId",
                table: "AttendanceRecords");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceRecords_CourseId_Date",
                table: "AttendanceRecords");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceRecords_StudentProfileId_Date",
                table: "AttendanceRecords");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_CourseId",
                table: "AttendanceRecords",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_StudentProfileId",
                table: "AttendanceRecords",
                column: "StudentProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRecords_Sections_SectionId",
                table: "AttendanceRecords",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRecords_Sections_SectionId",
                table: "AttendanceRecords");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceRecords_CourseId",
                table: "AttendanceRecords");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceRecords_StudentProfileId",
                table: "AttendanceRecords");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_CourseId_Date",
                table: "AttendanceRecords",
                columns: new[] { "CourseId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_StudentProfileId_Date",
                table: "AttendanceRecords",
                columns: new[] { "StudentProfileId", "Date" });

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRecords_Sections_SectionId",
                table: "AttendanceRecords",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
