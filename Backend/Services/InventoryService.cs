using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class InventoryService : IInventoryService
{
    private readonly AppDbContext _context;

    public InventoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Inventory>> GetAllAsync()
    {
        return await _context.Set<Inventory>().ToListAsync();
    }

    public async Task<Inventory?> GetByIdAsync(int id)
    {
        return await _context.Set<Inventory>().FindAsync(id);
    }

    public async Task CreateAsync(Inventory inventory)
    {
        _context.Set<Inventory>().Add(inventory);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, Inventory inventory)
    {
        var existing = await _context.Set<Inventory>().FindAsync(id);
        if (existing != null)
        {
            existing.Name = inventory.Name;
            existing.Description = inventory.Description;
            existing.Quantity = inventory.Quantity;
            existing.Price = inventory.Price;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Set<Inventory>().FindAsync(id);
        if (existing != null)
        {
            _context.Set<Inventory>().Remove(existing);
            await _context.SaveChangesAsync();
        }
    }
}
