using Backend.Models;

namespace Backend.Services;

public interface IRoleService
{
    Task<List<Role>> GetAllAsync();
    Task<Role?>      GetByIdAsync(int id);
    Task             CreateAsync(Role role);
    Task             UpdateAsync(int id, Role role);
    Task             DeleteAsync(int id);
}
