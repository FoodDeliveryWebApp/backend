namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

public interface IUserRepository
{
    bool Exists(string username);
    User? GetActiveByName(string username);
    User Create(User user);
    int GetPersonId(int userId);
    Task<User> CreateAsync(User user);
    Task<User?> GetByIdAsync(int id);
    Task UpdateAsync(User user);
    Person? GetPersonByUserId(int userId);
    Task<IEnumerable<User>> GetAllByRoleAsync(UserRole role);
    Task DeleteAsync(int userId);
}