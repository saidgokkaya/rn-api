using Core.Domain.Numarataj;
using Core.Domain.Ruhsat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.User
{
    public class Organization : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string TaskNumber { get; set; }
        public string Phone { get; set; }
        public string? BelName { get; set; }
        public string? LogoUrl { get; set; }
        public string? BelBaskan { get; set; }
        public string? BelTitle { get; set; }
        public string? Content1 { get; set; }
        public string? Content2 { get; set; }
        public string? Content3 { get; set; }
        public string? NumaratajBelName { get; set; }
        public string? NumaratajPersonName { get; set; }
        public string BelBaskanTitle { get; set; }
        public string Content4 { get; set; }
        public bool NumaratajView { get; set; }
        public bool RuhsatView { get; set; }
        public string? Paraf { get; set; }
        public string? ParafTitle { get; set; }
        public virtual ICollection<User> User { get; set; }
        public virtual ICollection<Core.Domain.Ruhsat.Ruhsat> Ruhsat { get; set; }
        public virtual ICollection<RuhsatSinifi> RuhsatSinifi { get; set; }
        public virtual ICollection<FaaliyetKonusu> FaaliyetKonusu { get; set; }
        public virtual ICollection<Depo> Depo { get; set; }
        public virtual ICollection<DepoBilgi> DepoBilgi { get; set; }
        public virtual ICollection<Mahalle> Mahalle { get; set; }
        public virtual ICollection<Numarataj.Numarataj> Numarataj { get; set; }
        public virtual ICollection<Log> Log { get; set; }
    }
}
