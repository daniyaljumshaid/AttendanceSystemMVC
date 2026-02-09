-- Check teacher profiles
SELECT 'Teacher Profiles' as Info, * FROM TeacherProfiles;

-- Check course schedules
SELECT 'Course Schedules' as Info, 
       cs.Id, cs.CourseId, c.Name as CourseName, 
       cs.SectionId, sec.Name as SectionName,
       cs.SessionId, ses.Name as SessionName,
       cs.TeacherProfileId, cs.DayOfWeek, cs.StartTime, cs.EndTime
FROM CourseSchedules cs
LEFT JOIN Courses c ON cs.CourseId = c.Id
LEFT JOIN Sections sec ON cs.SectionId = sec.Id
LEFT JOIN Sessions ses ON cs.SessionId = ses.Id
LEFT JOIN TeacherProfiles tp ON cs.TeacherProfileId = tp.Id;

-- Check if teacher 1 has schedules
SELECT 'Teacher 1 Schedules' as Info, * FROM CourseSchedules WHERE TeacherProfileId = 1;
