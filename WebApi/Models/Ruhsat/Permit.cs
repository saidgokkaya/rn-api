using Core.Domain.Ruhsat;
using Core.Domain.User;

namespace WebApi.Models.Ruhsat
{
    public class Permit
    {
        public int Id { get; set; }
        public string IsActive { get; set; }
        public string? RuhsatNo { get; set; }
        public DateTime VerilisTarihi { get; set; } = DateTime.UtcNow;
        public string? TcKimlikNo { get; set; }
        public string? FullName { get; set; }
        public string? IsyeriUnvani { get; set; }
        public string FaaliyetKonusuName { get; set; }
        public string RuhsatTuruName { get; set; }
        public string? PhotoPath { get; set; }
        public string? ScannedFilePath { get; set; }
    }
}
