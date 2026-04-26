using Backend.Models;

namespace Backend.Services;

public interface IAccountService
{
    Task<List<Account>> GetAllAsync();
    Task<Account?>      GetByIdAsync(int id);
    Task<Account?>      GetByUsernameAsync(string username);
    Task<Account?>      AuthenticateAsync(string username, string password);
    Task                CreateAsync(Account account);
    Task                UpdateAsync(int id, Account account);
    Task                DeleteAsync(int id);
}
