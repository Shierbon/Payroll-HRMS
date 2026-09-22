using System.ComponentModel.DataAnnotations;

namespace PayrollHrms.DTOs
{
    public class UpdateEmployeeDto
    {
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? MiddleName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal DailyRate { get; set; }

        [Required]
        [RegularExpression("^(MWF|TTHS)$",
            ErrorMessage = "Working days must be either MWF or TTHS.")]
        public string WorkingDays { get; set; } = string.Empty;
    }
}