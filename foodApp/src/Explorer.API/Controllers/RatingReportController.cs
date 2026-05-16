using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Explorer.API.Controllers
{
    [Route("api/ratingReports")]
    [ApiController]
    public class RatingReportController : ControllerBase
    {
        private readonly IRatingReportService _ratingReportService;

        public RatingReportController(IRatingReportService ratingReportService)
        {
            _ratingReportService = ratingReportService;
        }

        // ➤ Kreira prijavu za neku ocenu (menadžer)
        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<RatingReportDto>> CreateReport([FromBody] RatingReportDto dto)
        {
            var report = await _ratingReportService.CreateReportAsync(dto);
            return CreatedAtAction(nameof(CreateReport), new { id = report.Id }, report);
        }

        // ➤ Administrator menja status prijave (pregled prijave)
        [HttpPut("{reportId}/status")]
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult<RatingReportDto>> UpdateReportStatus(int reportId, [FromBody] string newStatus)
        {
            var updatedReport = await _ratingReportService.UpdateReportStatusAsync(reportId, newStatus);
            return Ok(updatedReport);
        }

        // ➤ (Opcionalno) Pregled svih prijava
        [HttpGet]
        [Authorize(Roles = "administrator,manager")]
        public async Task<ActionResult<List<RatingReportDto>>> GetAllReports()
        {
            var reports = await _ratingReportService.GetAllReportsAsync();
            return Ok(reports);
        }
    }
}
