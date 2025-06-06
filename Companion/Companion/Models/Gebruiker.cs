using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companion.Models
{
    public class Gebruiker
    {
        public string Id { get; set; }

        public string UserName { get; set; } = default!;

        public string Email { get; set; } = default!;
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; } = default!;
        public string? PhoneNumber { get; set; }

        public string? SimpleHash { get; set; } = default!;


        public string Naam { get; set; } = default!;
        public string Voornaam { get; set; } = default!;

        public List<int>? Rolnrs { get; set; }

        public List<EventGebruiker> EventGebruikers { get; set; }

        public Gebruiker() { }

    }
}
