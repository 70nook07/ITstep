-- 1. Вивести всі можливі пари рядків викладачів та груп
SELECT 
    t.Name + ' ' + t.Surname AS TeacherName,
    g.Name AS GroupName
FROM Teachers t
CROSS JOIN Groups g
ORDER BY TeacherName, GroupName;

GO

-- 2. Вивести назви факультетів, на яких фонд фінансування кафедр перевищує фонд фінансування факультету
SELECT 
    f.Name AS FacultyName,
    SUM(d.Financing) AS TotalDepartmentFinancing,
    ff.Financing AS FacultyFinancing
FROM Faculties f
INNER JOIN FacultyFinancing ff ON f.Id = ff.FacultyId
INNER JOIN Departments d ON f.Id = d.FacultyId
GROUP BY f.Id, f.Name, ff.Financing
HAVING SUM(d.Financing) > ff.Financing;

GO

-- 3. Вивести прізвища кураторів груп та назви груп, які вони курують
SELECT 
    t.Surname AS CuratorSurname,
    g.Name AS GroupName
FROM Groups g
INNER JOIN Teachers t ON g.CuratorId = t.Id
ORDER BY t.Surname, g.Name;

GO

-- 4. Вивести прізвища викладачів, які читають лекції у групі «P107»
SELECT DISTINCT
    t.Surname AS TeacherSurname
FROM Teachers t
INNER JOIN Lectures l ON t.Id = l.TeacherId
INNER JOIN Groups g ON l.GroupId = g.Id
WHERE g.Name = 'P107'
ORDER BY t.Surname;

GO

-- 5. Вивести прізвища викладачів та назви факультетів, на яких вони читають лекції
SELECT DISTINCT
    t.Surname AS TeacherSurname,
    f.Name AS FacultyName
FROM Teachers t
INNER JOIN Lectures l ON t.Id = l.TeacherId
INNER JOIN Groups g ON l.GroupId = g.Id
INNER JOIN Faculties f ON g.FacultyId = f.Id
ORDER BY t.Surname, f.Name;

GO

-- 6. Виведіть назви кафедр та назви груп, які до них відносяться
SELECT 
    d.Name AS DepartmentName,
    g.Name AS GroupName
FROM Departments d
INNER JOIN Groups g ON d.Id = g.DepartmentId
ORDER BY d.Name, g.Name;

GO

-- 7. Виведіть назви предметів, які викладає викладач «Samantha Adams»
-- Припускаємо таблицю Subjects та Lectures
SELECT DISTINCT
    s.Name AS SubjectName
FROM Subjects s
INNER JOIN Lectures l ON s.Id = l.SubjectId
INNER JOIN Teachers t ON l.TeacherId = t.Id
WHERE t.Name = 'Samantha' AND t.Surname = 'Adams'
ORDER BY s.Name;

GO

-- 8. Виведіть назви кафедр, на яких викладається предмет «Database Theory»
SELECT DISTINCT
    d.Name AS DepartmentName
FROM Departments d
INNER JOIN Teachers t ON d.Id = t.DepartmentId
INNER JOIN Lectures l ON t.Id = l.TeacherId
INNER JOIN Subjects s ON l.SubjectId = s.Id
WHERE s.Name = 'Database Theory'
ORDER BY d.Name;

GO

-- 9. Виведіть назви груп, які належать до факультету «Computer Science»
SELECT 
    g.Name AS GroupName
FROM Groups g
INNER JOIN Faculties f ON g.FacultyId = f.Id
WHERE f.Name = 'Faculty of Computer Science'
ORDER BY g.Name;

GO

-- 10. Виведіть назви груп 5-го курсу, а також назви факультетів, до яких вони відносяться
SELECT 
    g.Name AS GroupName,
    f.Name AS FacultyName
FROM Groups g
INNER JOIN Faculties f ON g.FacultyId = f.Id
WHERE g.Year = 5
ORDER BY f.Name, g.Name;

GO

-- 11. Вивести прізвища викладачів та лекції, які вони читають в аудиторії «B103»
SELECT 
    t.Surname AS TeacherSurname,
    s.Name AS SubjectName,
    g.Name AS GroupName
FROM Teachers t
INNER JOIN Lectures l ON t.Id = l.TeacherId
INNER JOIN Subjects s ON l.SubjectId = s.Id
INNER JOIN Groups g ON l.GroupId = g.Id
INNER JOIN Classrooms c ON l.ClassroomId = c.Id
WHERE c.RoomNumber = 'B103'
ORDER BY t.Surname, s.Name;
