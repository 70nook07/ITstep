-- 1. Вивести прізвища викладачів, які не читають лекції у понеділок
SELECT DISTINCT t.Surname
FROM Teachers t
WHERE t.Id NOT IN (
    SELECT l.TeacherId
    FROM Lectures l
    INNER JOIN Schedule s ON l.Id = s.LectureId
    WHERE s.DayOfWeek = 1
)

GO

-- 2. Вивести назви аудиторій, із зазначенням їх корпусів, у яких немає лекцій у середу другого тижня на третій парі
SELECT lr.Building, lr.Name
FROM LectureRooms lr
WHERE lr.Id NOT IN (
    SELECT s.LectureRoomId
    FROM Schedule s
    WHERE s.DayOfWeek = 3 
    AND s.Week = 2 
    AND s.Class = 3
)

GO

-- 3. Вивести повні імена викладачів факультету «Computer Science», які не курирують групи кафедри «Software Development»
SELECT t.Name, t.Surname
FROM Teachers t
WHERE t.Id IN (
    SELECT d.HeadId
    FROM Departments d
    INNER JOIN Faculties f ON d.FacultyId = f.Id
    WHERE f.Name = 'Computer Science'
)
AND t.Id NOT IN (
    SELECT gc.CuratorId
    FROM GroupsCurators gc
    INNER JOIN Groups g ON gc.GroupId = g.Id
    INNER JOIN Departments d ON g.DepartmentId = d.Id
    WHERE d.Name = 'Software Development'
)

GO

-- 4. Вивести список номерів усіх корпусів, які є у таблицях факультетів, кафедр та аудиторій
SELECT Building FROM Faculties
UNION
SELECT Building FROM Departments
UNION
SELECT Building FROM LectureRooms

GO

-- 5. Вивести повні імена викладачів у такому порядку: декани факультетів, завідувачі кафедр, викладачі, куратори, асистенти.
SELECT Name, Surname FROM Teachers WHERE Id IN (SELECT DeanId FROM Faculties)
UNION ALL
SELECT Name, Surname FROM Teachers WHERE Id IN (SELECT HeadId FROM Departments)
UNION ALL
SELECT Name, Surname FROM Teachers WHERE Id NOT IN (
    SELECT DeanId FROM Faculties 
    UNION 
    SELECT HeadId FROM Departments
    UNION 
    SELECT CuratorId FROM GroupsCurators
    UNION
    SELECT TeacherId FROM Deads
)
UNION ALL
SELECT Name, Surname FROM Teachers WHERE Id IN (SELECT CuratorId FROM GroupsCurators)
UNION ALL
SELECT Name, Surname FROM Teachers WHERE Id IN (SELECT TeacherId FROM Deads)

GO

-- 6. Вивести дні тижня (без повторень), в яких є заняття в аудиторіях «A311» та «A104» корпусу 6
SELECT DISTINCT s.DayOfWeek
FROM Schedule s
INNER JOIN LectureRooms lr ON s.LectureRoomId = lr.Id
WHERE lr.Building = 6 
AND lr.Name IN ('A311', 'A104')
