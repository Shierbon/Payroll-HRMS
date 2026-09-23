namespace PayrollHrms.DTOs
{
    public class PayrollResultDto
    {
        public string EmployeeNumber { get; set; } = string.Empty;

        public string EmployeeName { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal DailyRate { get; set; }

        public string WorkingDays { get; set; } = string.Empty;

        public int TotalWorkingDays { get; set; }

        public decimal BasicPay { get; set; }

        public decimal BirthdayPay { get; set; }

        public decimal TakeHomePay { get; set; }
    }
}