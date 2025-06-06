using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companion.Models
{
    public class EvenementGroup: List<Evenement>
    {
        public DateTime Datum { get; set; }
        public EvenementGroup(DateTime datum, List<Evenement> evenementen) : base(evenementen)
        {
            Datum = datum;
        }
    }
}
