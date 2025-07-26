using Core.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Ruhsat
{
    public class DepoBilgi : BaseEntity
    {
        public int RuhsatId { get; set; }
        public virtual Ruhsat Ruhsat { get; set; }

        public int DepoId { get; set; }
        public virtual Depo Depo { get; set; }

        public string DepoAdi { get; set; }

        public string Bilgi { get; set; }

        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
