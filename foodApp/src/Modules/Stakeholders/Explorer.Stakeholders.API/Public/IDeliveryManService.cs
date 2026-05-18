using Explorer.Stakeholders.API.Dtos;
using FluentResults;

namespace Explorer.Stakeholders.API.Public;

public interface IDeliveryManService
{
    Task<Result<IEnumerable<UserDto>>> GetAllAsync();
    Task<Result> CreateAsync(UserDto dto);
    Task<Result> UpdateAsync(int id, UserDto dto);
    Task<Result> DeleteAsync(int id);
}
