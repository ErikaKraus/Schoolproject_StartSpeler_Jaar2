using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace API.Models
{

    public class Community
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Naam { get; set; } = default!;
        
        public string? Afbeelding {  get; set; }

        public string? AfbeeldingPad => $"Resources/Images/{Afbeelding}";


        //Navigatieproperty
        [JsonIgnore]
        public List<Evenement> Evenementen { get; set; }     

    }
}
