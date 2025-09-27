-- 1. Вивести таблицю кафедр, але розташувати її поля у зворотному порядку.
SELECT Name
	 , Financing
	 , Id 
FROM Departments;
GO

-- 2. Вивести назви груп та їх рейтинги, використовуючи, як назви полів, що виводяться, «Group Name» та «Group Rating» відповідно.
SELECT Name AS [Group Name]
	 , Rating AS [Group Rating] 
FROM Groups;
GO

-- 3. Вивести для викладачів їхнє прізвище, відсоток ставки по відношенню до надбавки та відсоток ставки по відношенню до зарплати (сума ставки та надбавки).
SELECT Surname
     , CAST((Salary / NULLIF(Premium, 0)) * 100 AS DECIMAL(10,2)) AS [SalaryToPremiumPercent]
     , CAST((Salary / NULLIF(Salary + Premium, 0)) * 100 AS DECIMAL(10,2)) AS [SalaryToTotalPercent]
FROM Teachers;
GO

-- 4. Вивести таблицю факультетів у вигляді одного поля у такому форматі: «The dean of faculty [faculty] is [dean].».
SELECT 'The dean of faculty ' + Name + ' is ' + Dean + '.' AS FacultyInfo  -- Не маю поля dean
FROM Faculties;
GO

-- 5. Вивести прізвища викладачів, які є професорами та ставка яких перевищує 1050.
-- Припускаючи, що професори мають певну умову (наприклад, високу зарплату або інший критерій)
SELECT Surname 
FROM Teachers 
WHERE Salary > 1050 
GO

-- 6. Вивести назви кафедр, фонд фінансування яких менший за 11000 або більше 25000.
SELECT Name 
FROM Departments 
WHERE Financing < 11000 OR Financing > 25000;
GO

-- 7. Вивести назви факультетів, окрім факультету «Computer Science».
SELECT Name 
FROM Faculties 
WHERE Name <> 'Faculty of Computer Science';
GO

-- 8. Вивести прізвища та посади викладачів, які не є професорами.
-- Припускаючи, що посада зберігається в окремому полі (якого немає в оригінальній структурі)
SELECT Surname -- , Position 
FROM Teachers 
-- WHERE Position <> 'Professor'
GO

-- 9. Вивести прізвища, посади, ставки та надбавки асистентів, у яких надбавка у діапазоні від 160 до 550.
-- Припускаючи, що асистенти мають певну умову
SELECT Surname -- , Position, Salary, Premium 
FROM Teachers 
WHERE Premium BETWEEN 160 AND 550;
GO

-- 10. Вивести прізвища та ставки асистентів.
SELECT Surname, Salary 
FROM Teachers;
GO

-- 11. Вивести прізвища та посади викладачів, які були прийняті на роботу до 01.01.2000.
SELECT Surname -- , Position 
FROM Teachers 
WHERE EmploymentDate < '2000-01-01';
GO

-- 12. Вивести назви кафедр, які в алфавітному порядку розміщуються до кафедри «Software Development». 
SELECT Name AS [Name of Department] 
FROM Departments 
WHERE Name < 'Department of Software Engineering'
ORDER BY Name;
GO

-- 13. Вивести прізвища асистентів, які мають зарплату (сума ставки та надбавки) не більше 1200.
SELECT Surname 
FROM Teachers 
WHERE (Salary + Premium) <= 1200;
GO

-- 14. Вивести назви груп 5-го курсу, які мають рейтинг у діапазоні від 2 до 4.
SELECT Name 
FROM Groups 
WHERE Year = 5 AND Rating BETWEEN 2 AND 4;
GO

-- 15. Вивести прізвища асистентів зі ставкою менше 550 або надбавкою менше 200.
SELECT Surname 
FROM Teachers 
WHERE Salary < 550 OR Premium < 200;
GO
