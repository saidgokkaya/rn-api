using Core.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Ruhsat
{
    public class Depo : BaseEntity
    {
        public string Adi { get; set; }

        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        public int RuhsatSinifiId { get; set; }
        public virtual RuhsatSinifi RuhsatSinifi { get; set; }
        public virtual ICollection<DepoBilgi> DepoBilgi { get; set; }
    }
}
