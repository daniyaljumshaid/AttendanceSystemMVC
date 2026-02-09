using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceSystemMVC.Migrations
{
    /// <inheritdoc />
    public partial class FixCourseScheduleSessionRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRecords_Courses_CourseId",
                table: "AttendanceRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRecords_Sections_SectionId",
                table: "AttendanceRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRecords_StudentProfiles_StudentProfileId",
                table: "AttendanceRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseAssignments_Courses_CourseId",
                table: "CourseAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseAssignments_TeacherProfiles_TeacherProfileId",
                table: "CourseAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedules_Courses_CourseId",
                table: "CourseSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedules_Sections_SectionId",
                table: "CourseSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedules_TeacherProfiles_TeacherProfileId",
                table: "CourseSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Courses_CourseId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Sections_SectionId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Sessions_SessionId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_StudentProfiles_StudentProfileId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentProfiles_Sections_SectionId",
                table: "StudentProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherProfiles_AspNetUsers_ApplicationUserId",
                table: "TeacherProfiles");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "TeacherProfiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "StudentProfiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Sessions",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sessions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Sessions",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sections",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Courses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreditHours",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 3,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Courses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Courses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SectionSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    SessionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SectionSessions_Sections",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SectionSessions_Sessions",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseSchedules_SessionId",
                table: "CourseSchedules",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAssignments_SectionId",
                table: "CourseAssignments",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAssignments_SessionId",
                table: "CourseAssignments",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SectionSessions_SectionId",
                table: "SectionSessions",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SectionSessions_SessionId",
                table: "SectionSessions",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Course",
                table: "AttendanceRecords",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Section",
                table: "AttendanceRecords",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Student",
                table: "AttendanceRecords",
                column: "StudentProfileId",
                principalTable: "StudentProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignments_Courses",
                table: "CourseAssignments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignments_Sections",
                table: "CourseAssignments",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignments_Sessions",
                table: "CourseAssignments",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignments_TeacherProfiles",
                table: "CourseAssignments",
                column: "TeacherProfileId",
                principalTable: "TeacherProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedules_Courses",
                table: "CourseSchedules",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedules_Sections",
                table: "CourseSchedules",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedules_Sessions",
                table: "CourseSchedules",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedules_TeacherProfiles",
                table: "CourseSchedules",
                column: "TeacherProfileId",
                principalTable: "TeacherProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Courses",
                table: "Enrollments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Sections",
                table: "Enrollments",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Sessions",
                table: "Enrollments",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_StudentProfiles",
                table: "Enrollments",
                column: "StudentProfileId",
                principalTable: "StudentProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProfiles_Sections_SectionId",
                table: "StudentProfiles",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherProfile_AspNetUsers",
                table: "TeacherProfiles",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Course",
                table: "AttendanceRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Section",
                table: "AttendanceRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Student",
                table: "AttendanceRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseAssignments_Courses",
                table: "CourseAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseAssignments_Sections",
                table: "CourseAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseAssignments_Sessions",
                table: "CourseAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseAssignments_TeacherProfiles",
                table: "CourseAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedules_Courses",
                table: "CourseSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedules_Sections",
                table: "CourseSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedules_Sessions",
                table: "CourseSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedules_TeacherProfiles",
                table: "CourseSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Courses",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Sections",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Sessions",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_StudentProfiles",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentProfiles_Sections_SectionId",
                table: "StudentProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherProfile_AspNetUsers",
                table: "TeacherProfiles");

            migrationBuilder.DropTable(
                name: "SectionSessions");

            migrationBuilder.DropIndex(
                name: "IX_CourseSchedules_SessionId",
                table: "CourseSchedules");

            migrationBuilder.DropIndex(
                name: "IX_CourseAssignments_SectionId",
                table: "CourseAssignments");

            migrationBuilder.DropIndex(
                name: "IX_CourseAssignments_SessionId",
                table: "CourseAssignments");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Courses");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "TeacherProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "StudentProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Sessions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sessions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Sessions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sections",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreditHours",
                table: "Courses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRecords_Courses_CourseId",
                table: "AttendanceRecords",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRecords_Sections_SectionId",
                table: "AttendanceRecords",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRecords_StudentProfiles_StudentProfileId",
                table: "AttendanceRecords",
                column: "StudentProfileId",
                principalTable: "StudentProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignments_Courses_CourseId",
                table: "CourseAssignments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignments_TeacherProfiles_TeacherProfileId",
                table: "CourseAssignments",
                column: "TeacherProfileId",
                principalTable: "TeacherProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedules_Courses_CourseId",
                table: "CourseSchedules",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedules_Sections_SectionId",
                table: "CourseSchedules",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedules_TeacherProfiles_TeacherProfileId",
                table: "CourseSchedules",
                column: "TeacherProfileId",
                principalTable: "TeacherProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Courses_CourseId",
                table: "Enrollments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Sections_SectionId",
                table: "Enrollments",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Sessions_SessionId",
                table: "Enrollments",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_StudentProfiles_StudentProfileId",
                table: "Enrollments",
                column: "StudentProfileId",
                principalTable: "StudentProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProfiles_Sections_SectionId",
                table: "StudentProfiles",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherProfiles_AspNetUsers_ApplicationUserId",
                table: "TeacherProfiles",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
