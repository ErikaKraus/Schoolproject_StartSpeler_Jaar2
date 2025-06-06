using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Models
{
    public class Gebruiker
    {
        public string Id { get; set; }

        public string? UserName { get; set; }

        public string Email { get; set; } = default!;
        public bool EmailConfirmed { get; set; }
        public string? PasswordHash { get; set; }
        public string? PhoneNumber { get; set; }

        public string Naam { get; set; } = default!;
        public string Voornaam { get; set; } = default!;

        public string? SimpleHash { get; set; } = default!;
        public int AantalDeelnemers { get; set; }

        // navigation

        public List<int>? Rolnrs { get; set; }

        public List<Bestelling>? Bestellingen { get; set; }

        public List<EventGebruiker>? EventGebruikers { get; set; }



        public Gebruiker() { }                                     // constructor


    }
}