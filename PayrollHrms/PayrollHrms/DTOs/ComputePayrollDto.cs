using System.ComponentModel.DataAnnotations;

namespace PayrollHrms.DTOs
{
    public class ComputePayrollDto
    {
        [Required]
        public DateTime? StartDate { get; set; }

        [Required]
        public DateTime? EndDate { get; set; }
    }
}