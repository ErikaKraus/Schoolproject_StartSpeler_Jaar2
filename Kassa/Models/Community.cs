using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Models
{
    public class Community
    {
        public int Id { get; set; }
        public string Naam { get; set; } = default!;
        public string? Afbeelding { get; set; }

        //Navigatieproperty
        public List<Evenement> Evenementen { get; set; }

    }
}
