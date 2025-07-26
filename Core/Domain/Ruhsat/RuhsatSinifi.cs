using Core.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Ruhsat
{
    public class RuhsatSinifi : BaseEntity
    {
        public string Name { get; set; }

        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        public int RuhsatTuruId { get; set; }
        public virtual RuhsatTuru RuhsatTuru { get; set; }
        public virtual ICollection<Ruhsat> Ruhsat { get; set; }
        public virtual ICollection<Depo> Depo { get; set; }
    }
}
