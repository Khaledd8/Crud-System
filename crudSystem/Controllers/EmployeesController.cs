using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudSystem.Data;
using CrudSystem.Models;

namespace CrudSystem.Controllers
{
    [Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly string _uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        private readonly ILogger<EmployeesController> _logger;
    public EmployeesController(AppDbContext context, ILogger<EmployeesController> logger)
    {
        _context = context;
        _logger= logger;
    }

    // GET: api/employees
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employees>>> GetEmployees()
    {
        return await _context.Employees.ToListAsync();
    }

    // GET: api/employees/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Employees>> GetEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        return employee;
    }

    // POST: api/employees
    [Authorize(Roles = "User,Admin")]
    
        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromForm] Employees employee, [FromForm] IFormFile imageFile)
        {
            if (imageFile != null)
            {
                try
                {
                    if (!Directory.Exists(_uploadDirectory))
                    {
                        _logger.LogInformation("Creating uploads directory: {Directory}", _uploadDirectory);
                        Directory.CreateDirectory(_uploadDirectory);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(_uploadDirectory, fileName);

                    _logger.LogInformation("Saving file to path: {FilePath}", filePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    employee.ImagePath = fileName;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while uploading the file.");
                    return StatusCode(500, "Internal server error");
                }
            }

          _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

            return Ok(employee);
        }
    
    // PUT: api/employees/5
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEmployee(int id, [FromForm] Employees employee, IFormFile imageFile)
    {
        if (id != employee.Id)
        {
            return BadRequest();
        }

        if (imageFile != null)
        {
            var imagePath = Path.Combine("uploads", Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName));
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            employee.ImagePath = imagePath;
        }

        _context.Entry(employee).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EmployeeExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/employees/5
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
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

    private bool EmployeeExists(int id)
    {
        return _context.Employees.Any(e => e.Id == id);
    }
}

}
