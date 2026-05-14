namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IRestaurantApplicationRepository
    {
        Task<RestaurantApplication> CreateAsync(RestaurantApplication application);
        Task<RestaurantApplication?> GetByIdAsync(long id);
        Task<List<RestaurantApplication>> GetAllAsync();
        Task<List<RestaurantApplication>> GetByStatusAsync(ApplicationStatus status);
        Task<RestaurantApplication> UpdateAsync(RestaurantApplication application);
    }
}
