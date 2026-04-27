using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class AccountService : IAccountService
{
    private readonly AppDbContext _context;

    public AccountService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Account>> GetAllAsync()
    {
        return await _context.Accounts
            .Join(_context.Roles, 
                  a => a.RoleId, 
                  r => r.Id, 
                  (a, r) => new Account 
                  { 
                      Id = a.Id, 
                      Username = a.Username, 
                      Password = a.Password, 
                      Email = a.Email, 
                      RoleId = a.RoleId, 
                      RoleName = r.Name 
                  })
            .ToListAsync();
    }

    public async Task<Account?> GetByIdAsync(int id)
    {
        return await _context.Accounts
            .Where(a => a.Id == id)
            .Join(_context.Roles, 
                  a => a.RoleId, 
                  r => r.Id, 
                  (a, r) => new Account 
                  { 
                      Id = a.Id, 
                      Username = a.Username, 
                      Password = a.Password, 
                      Email = a.Email, 
                      RoleId = a.RoleId, 
                      RoleName = r.Name 
                  })
            .FirstOrDefaultAsync();
    }

    public async Task<Account?> GetByUsernameAsync(string username)
    {
        return await _context.Accounts
            .Where(a => a.Username == username)
            .Join(_context.Roles, 
                  a => a.RoleId, 
                  r => r.Id, 
                  (a, r) => new Account 
                  { 
                      Id = a.Id, 
                      Username = a.Username, 
                      Password = a.Password, 
                      Email = a.Email, 
                      RoleId = a.RoleId, 
                      RoleName = r.Name 
                  })
            .FirstOrDefaultAsync();
    }

    public async Task<Account?> AuthenticateAsync(string username, string password)
    {
        return await _context.Accounts
            .Where(a => a.Username == username && a.Password == password)
            .Join(_context.Roles, 
                  a => a.RoleId, 
                  r => r.Id, 
                  (a, r) => new Account 
                  { 
                      Id = a.Id, 
                      Username = a.Username, 
                      Password = a.Password, 
                      Email = a.Email, 
                      RoleId = a.RoleId, 
                      RoleName = r.Name 
                  })
            .FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Account account)
    {
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, Account account)
    {
        var existing = await _context.Accounts.FindAsync(id);
        if (existing != null)
        {
            existing.Username = account.Username;
            existing.Password = account.Password;
            existing.Email = account.Email;
            existing.RoleId = account.RoleId;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Accounts.FindAsync(id);
        if (existing != null)
        {
            _context.Accounts.Remove(existing);
            await _context.SaveChangesAsync();
        }
    }
}
