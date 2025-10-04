SELECT t.Name + ' ' + t.Surname AS FullName,
       COUNT(c.ClassID) AS LectureCount
FROM Teachers t
INNER JOIN Classes c ON t.TeacherID = c.TeacherID
WHERE t.Name + ' ' + t.Surname = 'Dave McQueen'
  AND c.ClassType = 'Lecture'
GROUP BY t.Name, t.Surname;

GO

SELECT	 COUNT(t.TeacherID)
FROM Teachers t 
INNER JOIN Departments d ON t.TeacherID = c.TeacherID
	WHERE d.DepartmentName = 'Software Development';
	
GO

SELECT COUNT(c.ClassID)
FROM Classes c
WHERE c.Classroom = 'D201';

GO

SELECT 
    t.Name + ' ' + t.Surname AS FullName,
    COUNT(DISTINCT l.SubjectId) AS SubjectsCount
FROM Teachers t
LEFT JOIN Lectures l ON t.TeacherID = l.TeacherID
GROUP BY t.TeacherID, t.Name, t.Surname

GO

SELECT 
    DATENAME(WEEKDAY, l.Date) AS DayOfWeek,
    l.Date AS LectureDate,
    COUNT(l.LectureID) AS LecturesCount
FROM Lectures l
WHERE l.Date BETWEEN DATEADD(DAY, 1-DATEPART(WEEKDAY, GETDATE()), GETDATE()) 
                 AND DATEADD(DAY, 7-DATEPART(WEEKDAY, GETDATE()), GETDATE())
GROUP BY l.Date, DATENAME(WEEKDAY, l.Date)

GO

SELECT 
    c.Classroom AS Classroom,
    COUNT(DISTINCT d.Id) AS DepartmentsCount
FROM Classrooms c
INNER JOIN Lectures l ON c.ClassroomID = l.ClassroomID
INNER JOIN Teachers t ON l.TeacherID = t.TeacherID
INNER JOIN Departments d ON t.DepartmentID = d.DepartmentID
GROUP BY c.Classroom




	
