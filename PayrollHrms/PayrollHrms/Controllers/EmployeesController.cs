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

        public EmployeesController(
            PayrollHrmsDbContext context,
            EmployeeNumberService employeeNumberService)
        {
            _context = context;
            _employeeNumberService = employeeNumberService;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await _context.Employees
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