using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Models
{
    public class AspnetUserRole
    {
        public string UserId { get; set; }
        public int RoleId { get; set; }
        // navigation
        public Gebruiker? Gebruiker { get; set; }
        public AspnetRole? AspnetRole { get; set; }
        public AspnetUserRole() { }


    }
}
