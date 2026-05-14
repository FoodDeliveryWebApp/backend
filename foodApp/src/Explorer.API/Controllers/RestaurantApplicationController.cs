using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers
{
    [Route("api/restaurant-applications")]
    [ApiController]
    public class RestaurantApplicationController : BaseApiController
    {
        private readonly IRestaurantApplicationService _service;

        public RestaurantApplicationController(IRestaurantApplicationService service)
        {
            _service = service;
        }

        // Anyone can submit an application (public, no auth required)
        [HttpPost]
        public async Task<ActionResult<RestaurantApplicationDto>> SubmitApplication([FromBody] RestaurantApplicationDto dto)
        {
            try
            {
                var result = await _service.SubmitApplicationAsync(dto);
                return CreatedAtAction(nameof(SubmitApplication), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Admin sees all applications
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<RestaurantApplicationDto>>> GetAllApplications()
        {
            var result = await _service.GetAllApplicationsAsync();
            return Ok(result);
        }

        // Admin sees only pending applications
        [HttpGet("pending")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<RestaurantApplicationDto>>> GetPendingApplications()
        {
            var result = await _service.GetPendingApplicationsAsync();
            return Ok(result);
        }

        // Admin approves or rejects an application
        [HttpPut("{id}/process")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<RestaurantApplicationDto>> ProcessApplication(long id, [FromBody] ProcessApplicationDto decision)
        {
            try
            {
                var result = await _service.ProcessApplicationAsync(id, decision);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
