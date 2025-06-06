using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companion.Models
{
    public class Community
    {
        public int Id { get; set; }
        public string Naam { get; set; } = default!;
        public string? Afbeelding { get; set; }

        public string AfbeeldingPad
        {
            get
            {
                var pad = Afbeelding != null ? $"Images/{Afbeelding}" : string.Empty;
                Debug.WriteLine($"AfbeeldingPad: {pad}");
                return pad;
            }
        }
        public List<Evenement> Evenementen { get; set; }

    }
}
