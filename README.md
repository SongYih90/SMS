# SMS
Salary Management System

Deploying application:

1.) Database
1.1) Run "DatabaseScript.sql" on sqlserver. It should create a new database 'SalaryManagement' with an 'Employees' table.

2.) Application
2.1) Open the application in visual studio.
2.2) Update connection string in web.config.
2.3) Build the application.
2.4) Add an application in IIS Default web site. Set the Alias as 'SalaryManagement', point the physical path to SalaryManagementSystem folder.
2.5) Access link: http://localhost/SalaryManagement/Employees

Available functions:
1.) Create new - Single creation
3.) Edit - Edit single record
2.) Upload - Upload CSV file
4.) Delete - Delete single record
5.) Search - Search by Employee ID, Name, Login name
6.) Filter - Filter by salary range
7.) Sort - Sort by individual columns
8.) Pagination - Each page contains 30 rows