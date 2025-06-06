using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Models
{
    public class UserRol
    {
        public int Id { get; set; }
        public int RoleId { get; set; }

        // navigation
        UserRol() { }                           // constructor

    }
}