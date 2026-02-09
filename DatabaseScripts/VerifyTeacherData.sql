-- Check which user is the teacher
SELECT 'Teacher Users' as Info, 
       u.Id as UserId, u.Email, u.FullName, u.IsApproved,
       tp.Id as TeacherProfileId, tp.EmployeeId
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
LEFT JOIN TeacherProfiles tp ON u.Id = tp.ApplicationUserId
WHERE r.Name = 'Teacher';

-- Check if there are any teacher profiles without matching users
SELECT 'Orphan Teacher Profiles' as Info, * 
FROM TeacherProfiles tp
WHERE NOT EXISTS (
    SELECT 1 FROM AspNetUsers u WHERE u.Id = tp.ApplicationUserId
);

-- Verify course schedules have proper relationships
SELECT 'Course Schedule Details' as Info,
       cs.Id, cs.CourseId, cs.TeacherProfileId, cs.SectionId, cs.SessionId,
       c.Name as CourseName,
       s.Name as SectionName,
       ses.Name as SessionName,
       tp.EmployeeId as TeacherEmployeeId
FROM CourseSchedules cs
LEFT JOIN Courses c ON cs.CourseId = c.Id
LEFT JOIN Sections s ON cs.SectionId = s.Id
LEFT JOIN Sessions ses ON cs.SessionId = ses.Id
LEFT JOIN TeacherProfiles tp ON cs.TeacherProfileId = tp.Id
WHERE cs.TeacherProfileId = 1;

-- Check if Course, Section, and Session records exist
SELECT 'Missing Courses' as Info, cs.CourseId 
FROM CourseSchedules cs
WHERE NOT EXISTS (SELECT 1 FROM Courses c WHERE c.Id = cs.CourseId);

SELECT 'Missing Sections' as Info, cs.SectionId
FROM CourseSchedules cs
WHERE NOT EXISTS (SELECT 1 FROM Sections s WHERE s.Id = cs.SectionId);

SELECT 'Missing Sessions' as Info, cs.SessionId
FROM CourseSchedules cs
WHERE NOT EXISTS (SELECT 1 FROM Sessions s WHERE s.Id = cs.SessionId);
