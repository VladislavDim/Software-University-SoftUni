USE SoftUni

UPDATE Employees
   SET Salary += Salary * 0.12
 WHERE DepartmentId IN(1, 2, 4, 11)

 
SELECT Salary
  FROM Employees
 WHERE DepartmentId IN(1, 2, 4, 11)
