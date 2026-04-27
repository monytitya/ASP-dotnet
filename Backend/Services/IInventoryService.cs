using Backend.Models;

namespace Backend.Services;

public interface IInventoryService
{
    Task<List<Inventory>> GetAllAsync();
    Task<Inventory?> GetByIdAsync(int id);
    Task CreateAsync(Inventory inventory);
    Task UpdateAsync(int id, Inventory inventory);
    Task DeleteAsync(int id);
}
