using Backend.Models;

namespace Backend.Services;

public interface IEmployeeService
{
    Task<List<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task CreateAsync(Employee emp);
    Task UpdateAsync(int id, Employee emp);
    Task DeleteAsync(int id);
}
