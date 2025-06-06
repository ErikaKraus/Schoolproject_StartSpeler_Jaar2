using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companion.Models
{
    public class Artikel
    {
        public int Id { get; set; }
        public string Naam { get; set; } = default!;
        public decimal Prijs { get; set; }
        public string Type { get; set; } = default!;
        public string? Logo { get; set; }
        public string? Info { get; set; }
        public int Voorraad { get; set; }
    }
}
