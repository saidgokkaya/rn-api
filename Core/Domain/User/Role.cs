﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.User
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }

        //1 user
        //2 admin
        //3 superadmin
        //4 usersession
        //5 ruhsat
        //6 numarataj
    }
}
