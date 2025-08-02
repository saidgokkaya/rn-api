using Core.Domain.User;

namespace WebApi.Models.Numarataj
{
    public class Numarataj
    {
        public int Id { get; set; }
        public string IsActive { get; set; }
        public string? TcKimlikNo { get; set; }
        public string? AdSoyad { get; set; }
        public string? Telefon { get; set; }
        public DateTime? InsertedDate { get; set; }
        public string? CaddeSokak { get; set; }
        public string? DisKapi { get; set; }
        public string? IcKapiNo { get; set; }
        public string NumaratajType { get; set; }
        public string Mahalle { get; set; }
    }
}
