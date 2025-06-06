using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        // navigation 
        Rol() { }                               // constructor
    }
}