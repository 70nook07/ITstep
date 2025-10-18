-- 1. Вивести назви груп 5-го курсу кафедри «Software Development», які мають понад 10 пар на перший тиждень
SELECT 
    g.Name AS GroupName
FROM Groups g
INNER JOIN Departments d ON g.DepartmentId = d.Id
INNER JOIN (
    SELECT GroupId, COUNT(*) AS LectureCount
    FROM Lectures 
    WHERE DATEPART(WEEK, Date) = DATEPART(WEEK, GETDATE())
    GROUP BY GroupId
    HAVING COUNT(*) > 10
) l ON g.Id = l.GroupId
WHERE g.Year = 5 
AND d.Name = 'Department of Software Engineering'
ORDER BY g.Name;

GO

-- 2. Вивести назви груп, які мають рейтинг більший, ніж рейтинг групи «D221»
SELECT 
    g1.Name AS GroupName,
    g1.Rating
FROM Groups g1
WHERE g1.Rating > (SELECT Rating FROM Groups WHERE Name = 'D221')
ORDER BY g1.Rating DESC;
GO

-- 3. Вивести прізвища та імена викладачів, ставка яких вища за середню ставку професорів
SELECT 
    t.Name,
    t.Surname,
    t.Salary
FROM Teachers t
WHERE t.Salary > (
    SELECT AVG(Salary) 
    FROM Teachers 
    WHERE Position = 'Professor'
)
ORDER BY t.Salary DESC;

GO

-- 4. Вивести назви груп, які мають більше одного куратора
SELECT 
    g.Name AS GroupName,
    COUNT(gc.TeacherId) AS CuratorsCount
FROM Groups g
INNER JOIN GroupCurators gc ON g.Id = gc.GroupId
GROUP BY g.Id, g.Name
HAVING COUNT(gc.TeacherId) > 1
ORDER BY CuratorsCount DESC;

GO

-- 5. Вивести назви груп, які мають рейтинг менший, ніж мінімальний рейтинг груп 5-го курсу
SELECT 
    g.Name AS GroupName,
    g.Rating
FROM Groups g
WHERE g.Rating < (SELECT MIN(Rating) FROM Groups WHERE Year = 5)
ORDER BY g.Rating;

GO

-- 6. Вивести назви факультетів, сумарний фонд фінансування кафедр яких більший за сумарний фонд фінансування кафедр факультету «Computer Science»
SELECT 
    f.Name AS FacultyName,
    SUM(d.Financing) AS TotalFinancing
FROM Faculties f
INNER JOIN Departments d ON f.Id = d.FacultyId
GROUP BY f.Id, f.Name
HAVING SUM(d.Financing) > (
    SELECT SUM(d2.Financing)
    FROM Departments d2
    INNER JOIN Faculties f2 ON d2.FacultyId = f2.Id
    WHERE f2.Name = 'Faculty of Computer Science'
)
ORDER BY TotalFinancing DESC;

GO

-- 7. Вивести назви дисциплін та повні імена викладачів, які читають найбільшу кількість лекцій
WITH TeacherLectureCount AS (
    SELECT 
        t.Id AS TeacherId,
        t.Name + ' ' + t.Surname AS TeacherName,
        s.Id AS SubjectId,
        s.Name AS SubjectName,
        COUNT(l.Id) AS LectureCount,
        RANK() OVER (PARTITION BY s.Id ORDER BY COUNT(l.Id) DESC) AS RankBySubject
    FROM Teachers t
    INNER JOIN Lectures l ON t.Id = l.TeacherId
    INNER JOIN Subjects s ON l.SubjectId = s.Id
    GROUP BY t.Id, t.Name, t.Surname, s.Id, s.Name
)
SELECT 
    SubjectName,
    TeacherName,
    LectureCount
FROM TeacherLectureCount
WHERE RankBySubject = 1
ORDER BY SubjectName, LectureCount DESC;

GO

-- 8. Вивести назву дисципліни, за якою читається найменше лекцій
SELECT TOP 1
    s.Name AS SubjectName,
    COUNT(l.Id) AS LectureCount
FROM Subjects s
LEFT JOIN Lectures l ON s.Id = l.SubjectId
GROUP BY s.Id, s.Name
ORDER BY COUNT(l.Id) ASC;

GO

-- 9. Вивести кількість студентів та дисциплін, що читаються на кафедрі «Software Development»
SELECT 
    d.Name AS DepartmentName,
    (SELECT COUNT(*) FROM Students s WHERE s.GroupId IN (
        SELECT g.Id FROM Groups g WHERE g.DepartmentId = d.Id
    )) AS StudentsCount,
    COUNT(DISTINCT l.SubjectId) AS SubjectsCount
FROM Departments d
LEFT JOIN Teachers t ON d.Id = t.DepartmentId
LEFT JOIN Lectures l ON t.Id = l.TeacherId
WHERE d.Name = 'Department of Software Engineering'
GROUP BY d.Id, d.Name;

GO
