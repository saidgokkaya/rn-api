using Core.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Ruhsat
{
    public class FaaliyetKonusu : BaseEntity
    {
        public string Name { get; set; }

        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        
        public virtual ICollection<Ruhsat> Ruhsat { get; set; }
    }
}
