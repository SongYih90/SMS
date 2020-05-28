
select * from dbo.employees WHERE (LoginName like '%s%')


UPDATE Employees SET LoginName = '', name = '', Salary = '' WHERE EmployeeID = ''

INSERT INTO Customers (CustomerName, ContactName, Address, City, PostalCode, Country)
VALUES ('Cardinal', 'Tom B. Erichsen', 'Skagen 21', 'Stavanger', '4006', 'Norway');

SELECT * FROM Employees WHERE EmployeeID = 'e0001'

ALTER TABLE Employees
ADD CONSTRAINT <CONSTRAINT_NAME> PRIMARY KEY(<COLUMN_NAME>)

e0001	hpotter
e0002	rwesley
e0003	ssnape
e0004	rhagrid
e0005	voldemort
e0006	gwesley
e0007	hgranger
e0008	adumbledore
e0009	dmalfoy
e0010	basilisk
e0011	sy

e0001	hpotter	Harry Potter	1235.00
e0002	rwesley	Ron Weasley	19234.50
e0003	ssnape	Severus Snape	4000.00
e0004	rhagrid	Rubeus Hagrid	4000.00
e0005	voldemort	Lord Voldemort	523.40
e0006	gwesley	Ginny Weasley	4000.00
e0007	hgranger	Hermione Granger	0.00
e0008	adumbledore	Albus Dumbledore	34.23
e0009	dmalfoy	Draco Malfoy	34234.50
e0010	basilisk	Basilisk	-23.43
e0011	sy	SYL	9898989.99

drop table Employees