using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AttendanceSystemMVC.Data;
using AttendanceSystemMVC.Models;
using AttendanceSystemMVC.ViewModels;

namespace AttendanceSystemMVC.ReportService
{
    public class ReportsService
    {
        private readonly ApplicationDbContext _context;

        public ReportsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StudentMonthlyReportVm> GetStudentMonthlyReport(
            int studentProfileId, int year, int month)
        {
            var from = new DateTime(year, month, 1);
            var to = from.AddMonths(1).AddDays(-1);

            var student = await _context.StudentProfiles
                .FirstOrDefaultAsync(s => s.Id == studentProfileId);

            var records = await _context.AttendanceRecords
                .Where(a =>
                    a.StudentProfileId == studentProfileId &&
                    a.Date >= from &&
                    a.Date <= to)
                .ToListAsync();

            var totalClasses = records.Count;
            var totalPresent = records.Count(r =>
                r.Status == AttendanceStatus.Present ||
                r.Status == AttendanceStatus.Late);
            var totalAbsent = records.Count(r =>
                r.Status == AttendanceStatus.Absent);
            var totalLate = records.Count(r =>
                r.Status == AttendanceStatus.Late);

            var percent = totalClasses == 0
                ? 0
                : ((double)totalPresent / totalClasses) * 100.0;

            return new StudentMonthlyReportVm
            {
                Student = student,
                Year = year,
                Month = month,
                TotalClasses = totalClasses,
                TotalPresent = totalPresent,
                TotalAbsent = totalAbsent,
                TotalLate = totalLate,
                OverallAttendancePercentage = percent
            };
        }

        public async Task<ClassReportVm> GetClassMonthlyReport(int courseId, int sectionId, int year, int month)
        {
            // Find the earliest attendance record date for this course/section
            var earliestRecord = await _context.AttendanceRecords
                .Where(a => a.CourseId == courseId && a.SectionId == sectionId)
                .OrderBy(a => a.Date)
                .FirstOrDefaultAsync();

            // If no records exist, return empty report
            if (earliestRecord == null)
            {
                return new ClassReportVm
                {
                    TotalDays = 0,
                    Present = 0,
                    Absent = 0,
                    Percentage = 0,
                    CourseId = courseId,
                    SectionId = sectionId,
                    Year = year,
                    Month = month,
                    MonthlyBreakdowns = new List<MonthlyBreakdown>()
                };
            }

            // If year and month are not provided or are default (current date), use earliest record date
            var startDate = new DateTime(year, month, 1);
            var earliestDate = new DateTime(earliestRecord.Date.Year, earliestRecord.Date.Month, 1);

            // Use earliest record date if the provided date is before it
            if (startDate < earliestDate)
            {
                startDate = earliestDate;
                year = earliestDate.Year;
                month = earliestDate.Month;
            }

            // Get 4 months of data starting from the determined date
            var monthlyBreakdowns = new List<MonthlyBreakdown>();
            int totalDaysAll = 0;
            int totalPresentAll = 0;
            int totalAbsentAll = 0;

            for (int i = 0; i < 4; i++)
            {
                var currentDate = startDate.AddMonths(i);
                var from = new DateTime(currentDate.Year, currentDate.Month, 1);
                var to = from.AddMonths(1).AddDays(-1);

                var records = await _context.AttendanceRecords
                    .Where(a => a.CourseId == courseId && a.SectionId == sectionId && a.Date >= from && a.Date <= to)
                    .ToListAsync();

                var total = records.Count;
                var present = records.Count(r => r.Status == AttendanceStatus.Present || r.Status == AttendanceStatus.Late);
                var absent = records.Count(r => r.Status == AttendanceStatus.Absent);
                var percent = total == 0 ? 0 : ((double)present / total) * 100.0;

                monthlyBreakdowns.Add(new MonthlyBreakdown
                {
                    Year = currentDate.Year,
                    Month = currentDate.Month,
                    MonthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentDate.Month),
                    TotalDays = total,
                    Present = present,
                    Absent = absent,
                    Percentage = percent
                });

                totalDaysAll += total;
                totalPresentAll += present;
                totalAbsentAll += absent;
            }

            var overallPercent = totalDaysAll == 0 ? 0 : ((double)totalPresentAll / totalDaysAll) * 100.0;

            return new ClassReportVm
            {
                TotalDays = totalDaysAll,
                Present = totalPresentAll,
                Absent = totalAbsentAll,
                Percentage = overallPercent,
                CourseId = courseId,
                SectionId = sectionId,
                Year = year,
                Month = month,
                MonthlyBreakdowns = monthlyBreakdowns
            };
        }

        public async Task<ClassReportVm> GetClassSemesterReport(int courseId, int sectionId, int sessionId)
        {
            var session = await _context.Sessions.FindAsync(sessionId);
            if (session == null) return new ClassReportVm();

            // Find the earliest attendance record date for this course/section WITHIN THE SESSION
            var earliestRecord = await _context.AttendanceRecords
                .Where(a => a.CourseId == courseId &&
                           a.SectionId == sectionId &&
                           a.Date >= session.StartDate &&
                           a.Date <= session.EndDate)
                .OrderBy(a => a.Date)
                .FirstOrDefaultAsync();

            // If no records exist within the session, return empty report
            if (earliestRecord == null)
            {
                return new ClassReportVm
                {
                    TotalDays = 0,
                    Present = 0,
                    Absent = 0,
                    Percentage = 0,
                    CourseId = courseId,
                    SectionId = sectionId,
                    SessionId = sessionId,
                    MonthlyBreakdowns = new List<MonthlyBreakdown>()
                };
            }

            // Start from the session start date or earliest record, whichever is later
            var from = earliestRecord.Date > session.StartDate
                ? new DateTime(earliestRecord.Date.Year, earliestRecord.Date.Month, 1)
                : new DateTime(session.StartDate.Year, session.StartDate.Month, 1);

            // End at the session end date
            var to = session.EndDate;

            // Calculate monthly breakdowns from session start to session end
            var monthlyBreakdowns = new List<MonthlyBreakdown>();
            int totalDaysAll = 0;
            int totalPresentAll = 0;
            int totalAbsentAll = 0;

            // Calculate how many months the session spans
            var currentDate = from;
            while (currentDate <= to)
            {
                var monthFrom = new DateTime(currentDate.Year, currentDate.Month, 1);
                var monthTo = monthFrom.AddMonths(1).AddDays(-1);

                // Clip to session dates
                if (monthFrom < session.StartDate) monthFrom = session.StartDate;
                if (monthTo > session.EndDate) monthTo = session.EndDate;

                var records = await _context.AttendanceRecords
                    .Where(a => a.CourseId == courseId &&
                               a.SectionId == sectionId &&
                               a.Date >= monthFrom &&
                               a.Date <= monthTo)
                    .ToListAsync();

                var total = records.Count;
                var present = records.Count(r => r.Status == AttendanceStatus.Present || r.Status == AttendanceStatus.Late);
                var absent = records.Count(r => r.Status == AttendanceStatus.Absent);
                var percent = total == 0 ? 0 : ((double)present / total) * 100.0;

                monthlyBreakdowns.Add(new MonthlyBreakdown
                {
                    Year = currentDate.Year,
                    Month = currentDate.Month,
                    MonthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentDate.Month),
                    TotalDays = total,
                    Present = present,
                    Absent = absent,
                    Percentage = percent
                });

                totalDaysAll += total;
                totalPresentAll += present;
                totalAbsentAll += absent;

                // Move to next month
                currentDate = currentDate.AddMonths(1);
            }

            var overallPercent = totalDaysAll == 0 ? 0 : ((double)totalPresentAll / totalDaysAll) * 100.0;

            return new ClassReportVm
            {
                TotalDays = totalDaysAll,
                Present = totalPresentAll,
                Absent = totalAbsentAll,
                Percentage = overallPercent,
                CourseId = courseId,
                SectionId = sectionId,
                SessionId = sessionId,
                MonthlyBreakdowns = monthlyBreakdowns
            };
        }
    }
}
