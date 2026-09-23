using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayrollHrms.Migrations
{
    /// <inheritdoc />
    public partial class CreateSampleEmployees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS
                (
                    SELECT 1
                    FROM dbo.Employees
                    WHERE EmployeeNumber = 'DEL-00001-17MAY1994'
                )
                BEGIN
                    INSERT INTO dbo.Employees
                    (
                        EmployeeNumber,
                        LastName,
                        FirstName,
                        MiddleName,
                        DateOfBirth,
                        DailyRate,
                        WorkingDays
                    )
                    VALUES
                    (
                        'DEL-00001-17MAY1994',
                        'DELA CRUZ',
                        'JUAN',
                        NULL,
                        '1994-05-17',
                        2000.00,
                        'MWF'
                    );
                END;

                IF NOT EXISTS
                (
                    SELECT 1
                    FROM dbo.Employees
                    WHERE EmployeeNumber = 'SY*-00002-01SEP1994'
                )
                BEGIN
                    INSERT INTO dbo.Employees
                    (
                        EmployeeNumber,
                        LastName,
                        FirstName,
                        MiddleName,
                        DateOfBirth,
                        DailyRate,
                        WorkingDays
                    )
                    VALUES
                    (
                        'SY*-00002-01SEP1994',
                        'SY',
                        'ANNIE',
                        NULL,
                        '1994-09-01',
                        1500.00,
                        'TTHS'
                    );
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DELETE FROM dbo.Employees
            WHERE EmployeeNumber IN
        (
            'DEL-00001-17MAY1994',
            'SY*-00002-01SEP1994'
        );
            ");
        }
    }
}
