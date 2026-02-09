-- Check Sessions
SELECT 'Sessions' as TableName, COUNT(*) as Count FROM Sessions;
SELECT * FROM Sessions;

-- Check Current Date Session
DECLARE @Today DATE = CAST(GETDATE() AS DATE);
SELECT 'Active Session Today' as Info, * FROM Sessions WHERE StartDate <= @Today AND EndDate >= @Today;

-- Check CourseSchedules
SELECT 'CourseSchedules' as TableName, COUNT(*) as Count FROM CourseSchedules;
SELECT TOP 10 cs.Id, cs.CourseId, c.Name as CourseName, cs.SectionId, sec.Name as SectionName, 
       cs.SessionId, ses.Name as SessionName, cs.DayOfWeek, cs.StartTime, cs.EndTime, cs.TeacherProfileId
FROM CourseSchedules cs
LEFT JOIN Courses c ON cs.CourseId = c.Id
LEFT JOIN Sections sec ON cs.SectionId = sec.Id
LEFT JOIN Sessions ses ON cs.SessionId = ses.Id;

-- Check Enrollments
SELECT 'Enrollments' as TableName, COUNT(*) as Count FROM Enrollments;
SELECT TOP 10 e.Id, e.CourseId, c.Name as CourseName, e.SectionId, sec.Name as SectionName,
       e.SessionId, ses.Name as SessionName, e.StudentProfileId, sp.RollNumber
FROM Enrollments e
LEFT JOIN Courses c ON e.CourseId = c.Id
LEFT JOIN Sections sec ON e.SectionId = sec.Id
LEFT JOIN Sessions ses ON e.SessionId = ses.Id
LEFT JOIN StudentProfiles sp ON e.StudentProfileId = sp.Id;

-- Check for ID mismatches
SELECT 'Enrollment Groups' as Info,
       e.CourseId, c.Name as CourseName, 
       e.SectionId, sec.Name as SectionName,
       e.SessionId, ses.Name as SessionName,
       COUNT(*) as StudentCount
FROM Enrollments e
LEFT JOIN Courses c ON e.CourseId = c.Id
LEFT JOIN Sections sec ON e.SectionId = sec.Id
LEFT JOIN Sessions ses ON e.SessionId = ses.Id
GROUP BY e.CourseId, c.Name, e.SectionId, sec.Name, e.SessionId, ses.Name;

-- Check Attendance Records for today
SELECT 'Attendance Today' as TableName, COUNT(*) as Count 
FROM AttendanceRecords 
WHERE Date = CAST(GETDATE() AS DATE);

SELECT TOP 10 * FROM AttendanceRecords WHERE Date = CAST(GETDATE() AS DATE);
