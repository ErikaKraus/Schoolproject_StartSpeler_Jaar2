using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{

    public class Artikel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Naam { get; set; } = default!;

        [Required]
        public decimal Prijs { get; set; }

        [Required]
        public string Type { get; set; } = default!;

        public string? Logo { get; set; }

        public string? Info { get; set; }


        [Required]
        public int Voorraad { get; set; }

        //Navigatieproperty

        [JsonIgnore]
        public List<BestellingLijn> Bestellinglijnen { get; set; }

    }
}
