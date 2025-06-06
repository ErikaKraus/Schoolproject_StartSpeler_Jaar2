using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace API.Models
{
    public partial class Bestelling
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime Tijdstip { get; set; }

        [Required]
        public int Tafelnummer { get; set; }

        [Required]
        public bool IsGeleverd { get; set; }

        [Required]
        public bool IsBetaald { get; set; }
        [Required]
        public bool IsOberBestelling { get; set; }

        public string? Commentaar { get; set; }
        [Required]
        public string GebruikerId { get; set; }                // FK

        //Navigatieproperty

       [JsonIgnore]
        public Gebruiker Gebruiker { get; set; }

        [JsonIgnore]
        public List<BestellingLijn> BestellingLijnen { get; set; }

    }
}
