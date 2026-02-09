using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceSystemMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentCourseAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentCourseAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentProfileId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourseAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentCourseAssignments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentCourseAssignments_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_StudentCourseAssignments_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentCourseAssignments_StudentProfiles_StudentProfileId",
                        column: x => x.StudentProfileId,
                        principalTable: "StudentProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_MarkedByTeacherId",
                table: "AttendanceRecords",
                column: "MarkedByTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseAssignments_CourseId",
                table: "StudentCourseAssignments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseAssignments_SectionId",
                table: "StudentCourseAssignments",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseAssignments_SessionId",
                table: "StudentCourseAssignments",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseAssignments_StudentProfileId",
                table: "StudentCourseAssignments",
                column: "StudentProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRecords_TeacherProfiles_MarkedByTeacherId",
                table: "AttendanceRecords",
                column: "MarkedByTeacherId",
                principalTable: "TeacherProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRecords_TeacherProfiles_MarkedByTeacherId",
                table: "AttendanceRecords");

            migrationBuilder.DropTable(
                name: "StudentCourseAssignments");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceRecords_MarkedByTeacherId",
                table: "AttendanceRecords");
        }
    }
}
