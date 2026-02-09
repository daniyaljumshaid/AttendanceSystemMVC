-- First, approve teacher 2 (hammad)
UPDATE AspNetUsers SET IsApproved = 1 WHERE Email = 'hammad@gmail.com';

-- Assign some courses to teacher 2 (hammad) - TeacherProfileId = 2
-- Copy some schedules from teacher 1 but with different times or days

-- Assign Functional English to teacher 2 on different days
INSERT INTO CourseSchedules (CourseId, TeacherProfileId, SectionId, SessionId, DayOfWeek, StartTime, EndTime)
VALUES 
(3, 2, 1, 4, 2, '09:00:00', '10:30:00'),  -- Tuesday
(3, 2, 1, 4, 4, '10:30:00', '12:00:00'),  -- Thursday
(3, 2, 1, 4, 6, '14:00:00', '15:30:00');  -- Saturday

-- Assign OOP to teacher 2
INSERT INTO CourseSchedules (CourseId, TeacherProfileId, SectionId, SessionId, DayOfWeek, StartTime, EndTime)
VALUES 
(1009, 2, 1, 4, 2, '10:30:00', '12:00:00'),  -- Tuesday
(1009, 2, 1, 4, 4, '14:00:00', '15:30:00'),   -- Thursday
(1009, 2, 1, 4, 6, '09:00:00', '10:30:00');   -- Saturday

-- Verify the changes
SELECT 'Updated Teacher 2' as Info, * FROM AspNetUsers WHERE Email = 'hammad@gmail.com';
SELECT 'Teacher 2 Schedules' as Info, 
       cs.*, c.Name as CourseName, s.Name as SectionName
FROM CourseSchedules cs
JOIN Courses c ON cs.CourseId = c.Id
JOIN Sections s ON cs.SectionId = s.Id
WHERE cs.TeacherProfileId = 2;
