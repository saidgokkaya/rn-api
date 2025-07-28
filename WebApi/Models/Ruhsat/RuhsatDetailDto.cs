namespace WebApi.Models.Ruhsat
{
    public class RuhsatDetailDto
    {
        public int Id { get; set; }
        public int RuhsatTuruId { get; set; }
        public int FaaliyetKonusuId { get; set; }
        public int? RuhsatSinifiId { get; set; }
        public string RuhsatNo { get; set; }
        public string TcKimlikNo { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string IsyeriUnvani { get; set; }
        public DateTime VerilisTarihi { get; set; }
        public string Ada { get; set; }
        public string Parsel { get; set; }
        public string Pafta { get; set; }
        public string Adres { get; set; }
        public string Not { get; set; }
        public Dictionary<int, WarehouseInfoDto> Warehouses { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class WarehouseInfoDto
    {
        public string Bilgi { get; set; }
        public string DepoAdi { get; set; }
    }
}
