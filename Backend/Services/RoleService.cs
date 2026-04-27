using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class RoleService : IRoleService
{
    private readonly AppDbContext _context;

    public RoleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Role>> GetAllAsync()
    {
        return await _context.Set<Role>().ToListAsync();
    }

    public async Task<Role?> GetByIdAsync(int id)
    {
        return await _context.Set<Role>().FindAsync(id);
    }

    public async Task CreateAsync(Role role)
    {
        _context.Set<Role>().Add(role);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, Role role)
    {
        var existing = await _context.Set<Role>().FindAsync(id);
        if (existing != null)
        {
            existing.Name = role.Name;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Set<Role>().FindAsync(id);
        if (existing != null)
        {
            _context.Set<Role>().Remove(existing);
            await _context.SaveChangesAsync();
        }
    }
}
