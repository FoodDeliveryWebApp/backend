using System;

namespace Explorer.Stakeholders.API.Dtos
{
    public class RatingReportDto
    {
        public int Id { get; set; }                // ID prijave
        public int OrderId { get; set; }           // ID porudžbine (da ne vraćamo ceo Order objekat)
        public int ManagerId { get; set; }         // ID menadžera koji je obrađuje
        public string Comment { get; set; }         // Komentar menadžera ili gosta
        public string Status { get; set; }          // Pending, Approved, Rejected
        public DateTime CreatedAt { get; set; }     // Datum i vreme kada je prijava napravljena
    }
}
