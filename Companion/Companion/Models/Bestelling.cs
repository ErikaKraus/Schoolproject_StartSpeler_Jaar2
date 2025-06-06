using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companion.Models
{
    public class Bestelling
    {
        public int Id { get; set; }
        public DateTime Tijdstip { get; set; }
        public int Tafelnummer { get; set; }
        public bool IsGeleverd { get; set; }
        public bool IsBetaald { get; set; }
        public bool IsOberBestelling { get; set; }
        public string? Commentaar { get; set; }
        public string? GebruikerId { get; set; }     // FK

    }
}
