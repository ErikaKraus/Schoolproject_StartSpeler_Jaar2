using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace API.Models
{
    
    public class Gebruiker : IdentityUser<string>
    {
                
        //Code voor kortere stringwaarde Id
        //public class Gebruiker : IdentityUser<string>
        //{

        [Required]
        public string Naam { get; set; } = default!;

        [Required]
        public string Voornaam { get; set; } = default!;

        public string? SimpleHash { get; set; } = default!;

        // Nullable datum/tijdvelden
        [DataType(DataType.DateTime)]
        public DateTime? LockoutEnd { get; set; }

        //Navigatieproperty

        [JsonIgnore]
        public List<Bestelling> Bestellingen { get; set; } = new List<Bestelling>();

        [JsonIgnore]
        public List<EventGebruiker> EventGebruikers { get; set; } = new List<EventGebruiker>();

        public override string Id { get; set; } = GenerateRandomId();

        private static string GenerateRandomId()
        {
            const string chars = "0123456789";                                      // aangepast zodat enkel numeriek

            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}

