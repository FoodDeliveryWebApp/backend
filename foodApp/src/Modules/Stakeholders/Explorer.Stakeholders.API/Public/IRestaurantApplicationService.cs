using Explorer.Stakeholders.API.Dtos;

namespace Explorer.Stakeholders.API.Public
{
    public interface IRestaurantApplicationService
    {
        Task<RestaurantApplicationDto> SubmitApplicationAsync(RestaurantApplicationDto dto);
        Task<List<RestaurantApplicationDto>> GetAllApplicationsAsync();
        Task<List<RestaurantApplicationDto>> GetPendingApplicationsAsync();
        Task<RestaurantApplicationDto> ProcessApplicationAsync(long applicationId, ProcessApplicationDto decision);
    }
}
