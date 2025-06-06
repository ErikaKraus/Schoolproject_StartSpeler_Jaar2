using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace API.Models
{
    public class Evenement
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Naam { get; set; } = default!;

        [Required]
        public DateTime Datum { get; set; }

        [Required]
        public TimeSpan Startuur { get; set; }

        [Required]
        public TimeSpan Einduur { get; set; }

        public decimal? Kostprijs { get; set; }
        
        public int? CommunityId { get; set; }            // FK

        [Required]
        public int MaxDeelnemersEvent { get; set; }

        public string? ExtraInfo { get; set; }

        //Navigatieproperty

        [JsonIgnore] 
        public Community Community { get; set; }

        [JsonIgnore]
        public List<EventGebruiker>? EventGebruikers { get; set; } = new List<EventGebruiker>();


    }
}

