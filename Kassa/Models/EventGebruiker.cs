using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Models
{
    public class EventGebruiker
    {
        public int Id { get; set; }
        public string GebruikerId { get; set; }
        public int EvenementId { get; set; }
        public int AantalDeelnemers { get; set; }

        // navigation
        public Evenement? Evenement { get; set; }
        public Gebruiker? Gebruiker { get; set; }

        public EventGebruiker() { }                            // constructor


    }
}