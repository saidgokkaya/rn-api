using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Ruhsat
{
    public class RuhsatTuru : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<RuhsatSinifi> RuhsatSinifi { get; set; }
        public virtual ICollection<Ruhsat> Ruhsat { get; set; }
    }
}
