using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace API.Models
{
    public class EventGebruiker
    {
        [Key]
        [Required]
        public int Id { get; set; }
       
        [Required]
        public string GebruikerId { get; set; }           // FK

        [Required]
        public int EvenementId { get; set; }                // FK

        [Required]
        public int AantalDeelnemers {  get; set; }

        public string? SpelerInformatie { get; set; }

        //Navigatieproperty
        [JsonIgnore]
        public Evenement? Evenement { get; set; } = default;

        [JsonIgnore]
        public Gebruiker? Gebruiker { get; set; } = default;
    }
}
