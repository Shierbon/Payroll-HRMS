using PayrollHrms.DTOs;
using PayrollHrms.Models;

namespace PayrollHrms.Services
{
    public class PayrollService
    {
        public PayrollResultDto ComputePay(
            Employee employee,
            DateTime startDate,
            DateTime endDate)
        {
            int totalWorkingDays = 0;

            decimal basicPay = 0;

            decimal birthdayPay = 0;

            // Loop through each date in the payroll period.
            for (DateTime date = startDate.Date;
                 date <= endDate.Date;
                 date = date.AddDays(1))
            {
                // Check if the date is a scheduled working day.
                bool isWorkingDay = IsWorkingDay(
                    employee.WorkingDays,
                    date.DayOfWeek);

                // Check if the date is the employee's birthday.
                bool isBirthday =
                    date.Month == employee.DateOfBirth.Month &&
                    date.Day == employee.DateOfBirth.Day;

                // RULE 1:
                // Regular working day = 200% of daily rate.
                if (isWorkingDay)
                {
                    totalWorkingDays++;

                    basicPay += employee.DailyRate * 2;
                }

                // RULE 2:
                // Birthday = additional 100% of daily rate.
                // Applies whether the employee works or not.
                if (isBirthday)
                {
                    birthdayPay += employee.DailyRate;
                }
            }

            // Calculate final take-home pay.
            decimal takeHomePay = basicPay + birthdayPay;

            // Return the payroll computation result.
            return new PayrollResultDto
            {
                EmployeeNumber = employee.EmployeeNumber,

                EmployeeName =
                    $"{employee.LastName}, {employee.FirstName}",

                StartDate = startDate.Date,

                EndDate = endDate.Date,

                DailyRate = employee.DailyRate,

                WorkingDays = employee.WorkingDays,

                TotalWorkingDays = totalWorkingDays,

                BasicPay = basicPay,

                BirthdayPay = birthdayPay,

                TakeHomePay = takeHomePay
            };
        }

        private bool IsWorkingDay(
            string workingDays,
            DayOfWeek day)
        {
            if (workingDays == "MWF")
            {
                return day == DayOfWeek.Monday ||
                       day == DayOfWeek.Wednesday ||
                       day == DayOfWeek.Friday;
            }

            if (workingDays == "TTHS")
            {
                return day == DayOfWeek.Tuesday ||
                       day == DayOfWeek.Thursday ||
                       day == DayOfWeek.Saturday;
            }

            return false;
        }
    }
}