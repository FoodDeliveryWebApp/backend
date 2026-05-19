using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Explorer.API.Controllers
{
    [Route("api/order-reports")]
    [ApiController]
    public class OrderReportController : ControllerBase
    {
        private readonly IOrderReportService _service;

        public OrderReportController(IOrderReportService service) => _service = service;

        [HttpPost]
        [Authorize(Roles = "guest")]
        public async Task<ActionResult<OrderReportDto>> CreateReport([FromBody] OrderReportDto dto)
        {
            var idClaim = User.FindFirstValue("id");
            if (!int.TryParse(idClaim, out var guestId))
                return Unauthorized();

            try
            {
                var report = await _service.CreateReportAsync(guestId, dto);
                return CreatedAtAction(nameof(CreateReport), new { id = report.Id }, report);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult<List<OrderReportDto>>> GetAllReports()
        {
            var reports = await _service.GetAllReportsAsync();
            return Ok(reports);
        }

        [HttpGet("my")]
        [Authorize(Roles = "guest")]
        public async Task<ActionResult<List<OrderReportDto>>> GetMyReports()
        {
            var idClaim = User.FindFirstValue("id");
            if (!int.TryParse(idClaim, out var guestId))
                return Unauthorized();

            var reports = await _service.GetGuestReportsAsync(guestId);
            return Ok(reports);
        }

        [HttpPut("{reportId}/answer")]
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult<OrderReportDto>> AnswerReport(int reportId, [FromBody] string answer)
        {
            try
            {
                var updated = await _service.AnswerReportAsync(reportId, answer);
                return Ok(updated);
            }
            catch (ArgumentException ex) { return NotFound(ex.Message); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
