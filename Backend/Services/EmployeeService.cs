using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class EmployeeService : IEmployeeService
{
    private readonly AppDbContext _context;

    public EmployeeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Employee>> GetAllAsync()
    {
        return await _context.Set<Employee>().ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Set<Employee>().FindAsync(id);
    }

    public async Task CreateAsync(Employee emp)
    {
        _context.Set<Employee>().Add(emp);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, Employee emp)
    {
        var existing = await _context.Set<Employee>().FindAsync(id);
        if (existing != null)
        {
            existing.Name = emp.Name;
            existing.Email = emp.Email;
            existing.Salary = emp.Salary;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Set<Employee>().FindAsync(id);
        if (existing != null)
        {
            _context.Set<Employee>().Remove(existing);
            await _context.SaveChangesAsync();
        }
    }
}
