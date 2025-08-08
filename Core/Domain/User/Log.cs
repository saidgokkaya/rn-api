using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.User
{
    public class Log : BaseEntity
    {
        public string ModuleName { get; set; }
        public string ProcessName { get; set; }
        public string BaseModule { get; set; }
        public int? TableId { get; set; }

        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        public int UserId { get; set; }
        public virtual Core.Domain.User.User User { get; set; }
    }
}
