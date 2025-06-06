using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Companion.Models
{
    public class Evenement
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("naam")]
        public string Naam { get; set; } = default!;

        [JsonPropertyName("datum")]
        public DateTime Datum { get; set; }

        [JsonPropertyName("startuur")]
        public TimeSpan Startuur { get; set; }

        [JsonPropertyName("einduur")]
        public TimeSpan Einduur { get; set; }

        [JsonPropertyName("kostprijs")]
        public decimal? Kostprijs { get; set; }

        [JsonPropertyName("communityid")]
        public int CommunityId { get; set; }            // FK

        [JsonPropertyName("maxdeelnemersevent")]
        public int MaxDeelnemersEvent { get; set; }

        public int BeschikbarePlaatsen { get; set; }

        [JsonPropertyName("extrainfo")]
        public string? ExtraInfo { get; set; }

        public Community Community { get; set; }

        public List<EventGebruiker> EventGebruikers { get; set; }
    }
}