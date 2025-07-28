using Microsoft.AspNetCore.Mvc;

namespace WebApi.Models.Ruhsat
{
    public class AddRequest
    {
        [FromForm(Name = "image")]
        public IFormFile Image { get; set; }

        [FromForm(Name = "activityId")]
        public int ActivityId { get; set; }

        [FromForm(Name = "turId")]
        public int TurId { get; set; }

        [FromForm(Name = "classId")]
        public int? ClassId { get; set; }

        [FromForm(Name = "ruhsatNo")]
        public string? RuhsatNo { get; set; }

        [FromForm(Name = "tcKimlikNo")]
        public string? TcKimlikNo { get; set; }

        [FromForm(Name = "adi")]
        public string? Adi { get; set; }

        [FromForm(Name = "soyadi")]
        public string? Soyadi { get; set; }

        [FromForm(Name = "isyeriUnvani")]
        public string? IsyeriUnvani { get; set; }

        [FromForm(Name = "verilisTarihi")]
        public DateTime VerilisTarihi { get; set; }

        [FromForm(Name = "ada")]
        public string? Ada { get; set; }

        [FromForm(Name = "parsel")]
        public string? Parsel { get; set; }

        [FromForm(Name = "pafta")]
        public string? Pafta { get; set; }

        [FromForm(Name = "adres")]
        public string? Adres { get; set; }

        [FromForm(Name = "not")]
        public string? Not { get; set; }

        [FromForm(Name = "warehouses")]
        public string? Warehouses { get; set; }
    }
}
