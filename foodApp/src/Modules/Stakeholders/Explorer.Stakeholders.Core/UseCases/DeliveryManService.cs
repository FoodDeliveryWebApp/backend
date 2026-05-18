using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases;

public class DeliveryManService : IDeliveryManService
{
    private readonly IUserRepository _userRepository;
    private readonly ICrudRepository<Person> _personRepository;

    public DeliveryManService(IUserRepository userRepository, ICrudRepository<Person> personRepository)
    {
        _userRepository = userRepository;
        _personRepository = personRepository;
    }

    public async Task<Result<IEnumerable<UserDto>>> GetAllAsync()
    {
        var users = await _userRepository.GetAllByRoleAsync(UserRole.DeliveryMan);
        var result = users.Select(u =>
        {
            var person = _userRepository.GetPersonByUserId(u.Id);
            return new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                IsActive = u.IsActive,
                Role = u.Role.ToString(),
                Name = person?.Name,
                Surname = person?.Surname,
                Email = person?.Email
            };
        }).ToList();
        return Result.Ok<IEnumerable<UserDto>>(result);
    }

    public async Task<Result> CreateAsync(UserDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username)) return Result.Fail("Username is required.");
        if (string.IsNullOrWhiteSpace(dto.Password)) return Result.Fail("Password is required.");
        if (string.IsNullOrWhiteSpace(dto.Email))   return Result.Fail("Email is required.");

        if (_userRepository.Exists(dto.Username))
            return Result.Fail("Username already taken.");

        try
        {
            var user = new User(dto.Username, dto.Password, UserRole.DeliveryMan, dto.IsActive);
            await _userRepository.CreateAsync(user);
            _personRepository.Create(new Person(user.Id, dto.Name ?? dto.Username, dto.Surname ?? "-", dto.Email));
            return Result.Ok();
        }
        catch (ArgumentException ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result> UpdateAsync(int id, UserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return Result.Fail("Delivery man not found.");
        if (user.Role != UserRole.DeliveryMan) return Result.Fail("User is not a delivery man.");

        user.IsActive = dto.IsActive;
        await _userRepository.UpdateAsync(user);

        var person = _userRepository.GetPersonByUserId(id);
        if (person != null)
        {
            if (dto.Name != null) person.Name = dto.Name;
            if (dto.Surname != null) person.Surname = dto.Surname;
            if (dto.Email != null) person.Email = dto.Email;
            _personRepository.Update(person);
        }

        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return Result.Fail("Delivery man not found.");
        if (user.Role != UserRole.DeliveryMan) return Result.Fail("User is not a delivery man.");

        await _userRepository.DeleteAsync(id);
        return Result.Ok();
    }
}
