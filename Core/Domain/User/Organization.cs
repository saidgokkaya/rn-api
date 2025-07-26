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
        public virtual ICollection<User> User { get; set; }
        public virtual ICollection<Core.Domain.Ruhsat.Ruhsat> Ruhsat { get; set; }
        public virtual ICollection<RuhsatSinifi> RuhsatSinifi { get; set; }
        public virtual ICollection<FaaliyetKonusu> FaaliyetKonusu { get; set; }
        public virtual ICollection<Depo> Depo { get; set; }
        public virtual ICollection<DepoBilgi> DepoBilgi { get; set; }
    }
}
