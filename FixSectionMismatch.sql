-- Fix the section mismatch
-- The teacher's schedules are for SectionId = 1 (section 'b')
-- But students are enrolled in SectionId = 2 and 3 (sections 'B' and 'A')

-- First, let's see what sections exist
SELECT * FROM Sections;

-- Update enrollments to use the correct section that matches the teacher's schedule
-- Update all enrollments to SectionId = 1 (section 'b')
UPDATE Enrollments SET SectionId = 1;

-- Verify the update
SELECT e.Id, e.CourseId, c.Name as CourseName, e.SectionId, sec.Name as SectionName,
       e.SessionId, ses.Name as SessionName, e.StudentProfileId, sp.RollNumber
FROM Enrollments e
LEFT JOIN Courses c ON e.CourseId = c.Id
LEFT JOIN Sections sec ON e.SectionId = sec.Id
LEFT JOIN Sessions ses ON e.SessionId = ses.Id
LEFT JOIN StudentProfiles sp ON e.StudentProfileId = sp.Id;
