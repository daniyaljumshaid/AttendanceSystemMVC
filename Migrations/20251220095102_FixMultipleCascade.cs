using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceSystemMVC.Migrations
{
    /// <inheritdoc />
    public partial class FixMultipleCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRecords_Sections_SectionId",
                table: "AttendanceRecords");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRecords_Sections_SectionId",
                table: "AttendanceRecords",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRecords_Sections_SectionId",
                table: "AttendanceRecords");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRecords_Sections_SectionId",
                table: "AttendanceRecords",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
