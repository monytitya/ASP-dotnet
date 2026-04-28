using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Services;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeesController(IEmployeeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(await _service.GetAllAsync());
        }
        catch (Exception ex)
        {
            var msg = ex.InnerException?.Message ?? ex.Message;
            Console.WriteLine($"Error getting all employees: {msg}");
            return StatusCode(500, new { message = msg });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var emp = await _service.GetByIdAsync(id);
            if (emp == null) return NotFound();
            return Ok(emp);
        }
        catch (Exception ex)
        {
            var msg = ex.InnerException?.Message ?? ex.Message;
            Console.WriteLine($"Error getting employee {id}: {msg}");
            return StatusCode(500, new { message = msg });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(Employee emp)
    {
        try
        {
            await _service.CreateAsync(emp);
            return Ok(new { message = "Created successfully" });
        }
        catch (Exception ex)
        {
            var msg = ex.InnerException?.Message ?? ex.Message;
            Console.WriteLine($"Error creating employee: {msg}");
            return StatusCode(500, new { message = msg });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Employee emp)
    {
        try
        {
            await _service.UpdateAsync(id, emp);
            return Ok(new { message = "Updated successfully" });
        }
        catch (Exception ex)
        {
            var msg = ex.InnerException?.Message ?? ex.Message;
            Console.WriteLine($"Error updating employee: {msg}");
            return StatusCode(500, new { message = msg });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return Ok(new { message = "Deleted successfully" });
        }
        catch (Exception ex)
        {
            var msg = ex.InnerException?.Message ?? ex.Message;
            Console.WriteLine($"Error deleting employee: {msg}");
            return StatusCode(500, new { message = msg });
        }
    }
}
