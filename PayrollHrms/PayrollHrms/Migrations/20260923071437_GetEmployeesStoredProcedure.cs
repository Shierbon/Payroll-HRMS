using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayrollHrms.Migrations
{
    public partial class GetEmployeesStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE dbo.uspGetEmployees
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT
                        Id,
                        EmployeeNumber,
                        LastName,
                        FirstName,
                        MiddleName,
                        DateOfBirth,
                        DailyRate,
                        WorkingDays
                    FROM dbo.Employees
                    ORDER BY Id;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS dbo.uspGetEmployees;
            ");
        }
    }
}