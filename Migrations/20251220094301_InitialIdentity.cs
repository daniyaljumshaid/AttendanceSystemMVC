using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceSystemMVC.Migrations
{
    /// <inheritdoc />
    public partial class InitialIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRecords_StudentProfiles_StudentProfileId",
                table: "AttendanceRecords");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRecords_StudentProfiles_StudentProfileId",
                table: "AttendanceRecords",
                column: "StudentProfileId",
                principalTable: "StudentProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRecords_StudentProfiles_StudentProfileId",
                table: "AttendanceRecords");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRecords_StudentProfiles_StudentProfileId",
                table: "AttendanceRecords",
                column: "StudentProfileId",
                principalTable: "StudentProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
