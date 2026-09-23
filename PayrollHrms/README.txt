PAYROLL HRMS - .NET DEVELOPER ASSESSMENT

HOW TO RUN AFTER CLONING

1.  Clone the GitHub repository, then open PayrollHrms.sln in Visual
    Studio 2022.

2.  Install Nuget packages if not installed yet -> Microsoft.EntityFrameworkCore.SqlServer and Microsoft.EntityFrameworkCore.Tools

3.  Open appsettings.json and update ConnectionStrings:DefaultConnection
    to match your SQL Server instance. Example for Windows
    Authentication:

    “ConnectionStrings”: 
    { “DefaultConnection”: “Server=YOUR-PC-NAME\SQLEXPRESS;Database=DBName;Trusted_Connection=True;TrustServerCertificate=True;”
    }

    -> Replace YOUR-PC-NAME\SQLEXPRESS with your actual SQL instance Server Name.
    -> Replace DBName with your actual SQL instance Database name 

4.  Set PayrollHrms as the startup project.

5.  Open Tools > NuGet Package Manager > Package Manager Console.

6.  Set the Package Manager Console’s Default project to PayrollHrms.

7.  Run:

    Update-Database

    This applies the EF Core migrations:
    -> Create Employees table
    -> Create dbo.uspGetEmployees Stored Procedure
    -> Create sample Employee data (Based on the 2 employee in PDF assessment)
    Note: You do not need to run the stored
    procedure creation script manually.

8.  Build the solution (Ctrl + Shift + B).

9.  Select the https launch profile and press F5 to run the application.