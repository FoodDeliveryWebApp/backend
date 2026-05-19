using System;

namespace Explorer.Stakeholders.API.Dtos
{
    public class OrderReportDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int GuestId { get; set; }
        public string? Description { get; set; }
        public string? Answer { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
