-- ========================================
-- FIX MISSING ENROLLMENTS
-- ========================================
-- This script creates Enrollment records for all StudentCourseAssignments 
-- that don't have corresponding Enrollments.
-- This fixes the issue where students don't appear in teacher attendance
-- and don't see classes in their dashboard.
-- ========================================

-- First, let's see what's missing
SELECT 
    'MISSING ENROLLMENTS - These assignments need enrollment records:' AS [Info];

SELECT 
    sca.Id AS AssignmentId,
    u.FullName AS StudentName,
    sp.RollNumber,
    c.Name AS CourseName,
    sec.Name AS SectionName,
    sess.Name AS SessionName,
    sca.StudentProfileId,
    sca.CourseId,
    sca.SectionId,
    sca.SessionId
FROM StudentCourseAssignments sca
INNER JOIN StudentProfiles sp ON sca.StudentProfileId = sp.Id
INNER JOIN AspNetUsers u ON sp.ApplicationUserId = u.Id
INNER JOIN Courses c ON sca.CourseId = c.Id
INNER JOIN Sections sec ON sca.SectionId = sec.Id
INNER JOIN Sessions sess ON sca.SessionId = sess.Id
WHERE NOT EXISTS (
    SELECT 1 
    FROM Enrollments e 
    WHERE e.StudentProfileId = sca.StudentProfileId 
        AND e.CourseId = sca.CourseId 
        AND e.SessionId = sca.SessionId
);

-- Now let's fix them by creating the missing Enrollment records
INSERT INTO Enrollments (StudentProfileId, CourseId, SectionId, SessionId)
SELECT DISTINCT 
    sca.StudentProfileId, 
    sca.CourseId, 
    sca.SectionId, 
    sca.SessionId
FROM StudentCourseAssignments sca
WHERE NOT EXISTS (
    SELECT 1 
    FROM Enrollments e 
    WHERE e.StudentProfileId = sca.StudentProfileId 
        AND e.CourseId = sca.CourseId 
        AND e.SessionId = sca.SessionId
);

-- Verify the fix
SELECT 
    '✓ FIX COMPLETE - Enrollment records created!' AS [Result],
    COUNT(*) AS [NewEnrollmentsCreated]
FROM Enrollments;

-- Show all current enrollments
SELECT 
    'CURRENT ENROLLMENTS:' AS [Info];
    
SELECT 
    e.Id,
    u.FullName AS StudentName,
    sp.RollNumber,
    c.Name AS CourseName,
    sec.Name AS SectionName,
    sess.Name AS SessionName
FROM Enrollments e
INNER JOIN StudentProfiles sp ON e.StudentProfileId = sp.Id
INNER JOIN AspNetUsers u ON sp.ApplicationUserId = u.Id
INNER JOIN Courses c ON e.CourseId = c.Id
INNER JOIN Sections sec ON e.SectionId = sec.Id
INNER JOIN Sessions sess ON e.SessionId = sess.Id
ORDER BY sp.RollNumber, c.Name;
