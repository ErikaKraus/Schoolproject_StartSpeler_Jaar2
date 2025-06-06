using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kassa.Models
{
    public class Evenement
    {
        public int Id { get; set; }
        public string Naam { get; set; } = default!;
        public DateTime Datum { get; set; }
        public TimeSpan Startuur { get; set; }
        public TimeSpan Einduur { get; set; }
        public decimal? Kostprijs { get; set; }
        public int? CommunityId { get; set; }            
        public int MaxDeelnemersEvent { get; set; }
        public string? ExtraInfo { get; set; }

        public Community Community { get; set; }

        public List<EventGebruiker> EventGebruikers { get; set; }


    }
}
