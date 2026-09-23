using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayrollHrms.Data;
using PayrollHrms.DTOs;
using PayrollHrms.Models;
using PayrollHrms.Services;


namespace PayrollHrms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly PayrollHrmsDbContext _context;
        private readonly EmployeeNumberService _employeeNumberService;
        private readonly PayrollService _payrollService;

        public EmployeesController(
            PayrollHrmsDbContext context,
            EmployeeNumberService employeeNumberService,
            PayrollService payrollService)
        {
            _context = context;
            _employeeNumberService = employeeNumberService;
            _payrollService = payrollService;
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(
            CreateEmployeeDto request)
        {
            var employee = new Employee
            {
                EmployeeNumber = _employeeNumberService.Generate(
                    request.LastName,
                    request.DateOfBirth),

                LastName = request.LastName.Trim(),
                FirstName = request.FirstName.Trim(),
                MiddleName = request.MiddleName?.Trim(),
                DateOfBirth = request.DateOfBirth,
                DailyRate = request.DailyRate,
                WorkingDays = request.WorkingDays.ToUpperInvariant()
            };

            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEmployee),
                new { id = employee.Id },
                employee);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEmployee(int id,UpdateEmployeeDto request)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            employee.LastName = request.LastName.Trim();
            employee.FirstName = request.FirstName.Trim();
            employee.MiddleName = request.MiddleName?.Trim();
            employee.DateOfBirth = request.DateOfBirth;
            employee.DailyRate = request.DailyRate;
            employee.WorkingDays = request.WorkingDays.ToUpperInvariant();

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id:int}/compute-pay")]
        public async Task<ActionResult<PayrollResultDto>> ComputePay(int id,ComputePayrollDto request)
        {
            // Validate required payroll dates.
            if (!request.StartDate.HasValue ||
                !request.EndDate.HasValue)
            {
                return BadRequest(
                    "Start date and end date are required.");
            }

            // Validate the payroll period.
            if (request.StartDate.Value.Date >
                request.EndDate.Value.Date)
            {
                return BadRequest(
                    "Start date cannot be greater than end date.");
            }

            // Retrieve employee from SQL Server.
            var employee = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            // Compute employee take-home pay.
            var result = _payrollService.ComputePay(
                employee,
                request.StartDate.Value,
                request.EndDate.Value);

            // Return the payroll result.
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await _context.Employees
                .FromSqlRaw("EXEC dbo.uspGetEmployees")
                .AsNoTracking()
                .ToListAsync();

            return Ok(employees);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }
    }
}