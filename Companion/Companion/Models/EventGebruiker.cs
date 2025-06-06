using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;


namespace Companion.Models
{
    public class EventGebruiker
    {
        public int Id { get; set; }

        [Required] 
        public string GebruikerId { get; set; } = default!;

        [Required] 
        public int EvenementId { get; set; }

        public int AantalDeelnemers { get; set; }

        public string? SpelerInformatie { get; set; }

        [JsonIgnore]
        public Evenement Evenement { get; set; }

        [JsonIgnore]
        public Gebruiker Gebruiker { get; set; }
    }
}
