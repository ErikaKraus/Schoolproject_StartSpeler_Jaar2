using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace API.Models
{
    public class BestellingLijn
    {
        [Key]
        [Required]
        public int Id { get; set; }
      
        [Required]
        public int Aantal { get; set; }

        [Required]
        public int ArtikelId { get; set; }              // FK
        [Required]
        public int BestellingId { get; set; }           // FK

        //Navigatieproperty
        [JsonIgnore]
        public Artikel Artikel { get; set; }

        [JsonIgnore]
        public Bestelling Bestelling { get; set; }

        
    }
}
