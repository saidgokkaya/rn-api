using Core.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Ruhsat
{
    public class Ruhsat : BaseEntity
    {
        public string? RuhsatNo { get; set; }
        public DateTime VerilisTarihi { get; set; } = DateTime.UtcNow;
        public string? TcKimlikNo { get; set; }
        public string? Adi { get; set; }
        public string? Soyadi { get; set; }
        public string? IsyeriUnvani { get; set; }

        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        public int FaaliyetKonusuId { get; set; }
        public virtual FaaliyetKonusu FaaliyetKonusu { get; set; }

        public int RuhsatTuruId { get; set; }
        public virtual RuhsatTuru RuhsatTuru { get; set; }

        public int? RuhsatSinifiId { get; set; }
        public virtual RuhsatSinifi? RuhsatSinifi { get; set; }

        public string? Adres { get; set; }
        public string? Not { get; set; }
        public string? PhotoPath { get; set; }
        public string? ScannedFilePath { get; set; }
        public string? Ada { get; set; }
        public string? Parsel { get; set; }
        public string? Pafta { get; set; }
        public virtual ICollection<DepoBilgi> DepoBilgi { get; set; }
    }
}
