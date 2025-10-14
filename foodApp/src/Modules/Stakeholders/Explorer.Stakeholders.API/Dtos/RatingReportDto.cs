using System;

namespace Explorer.Stakeholders.API.Dtos
{
    public class RatingReportDto
    {
        public long Id { get; set; }                // ID prijave
        public long OrderId { get; set; }           // ID porudžbine (da ne vraćamo ceo Order objekat)
        public long ManagerId { get; set; }         // ID menadžera koji je obrađuje
        public string Comment { get; set; }         // Komentar menadžera ili gosta
        public string Status { get; set; }          // Pending, Approved, Rejected
        public DateTime CreatedAt { get; set; }     // Datum i vreme kada je prijava napravljena
    }
}
