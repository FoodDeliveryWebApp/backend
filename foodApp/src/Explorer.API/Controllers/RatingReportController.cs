using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Explorer.API.Controllers
{
    [Route("api/ratingReports")]
    [ApiController]
    public class RatingReportController : ControllerBase
    {
        private readonly IRatingReportService _service;

        public RatingReportController(IRatingReportService service) => _service = service;

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<RatingReportDto>> CreateReport([FromBody] RatingReportDto dto)
        {
            var userIdClaim = User.FindFirstValue("id");
            if (!int.TryParse(userIdClaim, out var managerId))
                return Unauthorized("Invalid user.");

            try
            {
                var report = await _service.CreateReportAsync(managerId, dto);
                return CreatedAtAction(nameof(CreateReport), new { id = report.Id }, report);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult<List<RatingReportDto>>> GetAllReports()
        {
            var reports = await _service.GetAllReportsAsync();
            return Ok(reports);
        }

        [HttpGet("my")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<RatingReportDto>>> GetMyReports()
        {
            var userIdClaim = User.FindFirstValue("id");
            if (!int.TryParse(userIdClaim, out var managerId))
                return Unauthorized("Invalid user.");

            try
            {
                var reports = await _service.GetManagerReportsAsync(managerId);
                return Ok(reports);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{reportId}/status")]
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult<RatingReportDto>> UpdateReportStatus(int reportId, [FromBody] string newStatus)
        {
            try
            {
                var updated = await _service.UpdateReportStatusAsync(reportId, newStatus);
                return Ok(updated);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
