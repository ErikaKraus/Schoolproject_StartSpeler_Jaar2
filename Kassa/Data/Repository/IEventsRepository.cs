using Kassa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Data.Repository
{
    public interface IEventsRepository
    {
        public IEnumerable<Evenement> OphalenEvents();
        public IEnumerable<Evenement> OphalenEvenementenVoorCommunity(int communityId);

        public bool ToevoegenEvenement(Evenement evenement);
        public bool DuplicerenEvenement(Evenement evenement);
        public bool BewerkenEvenement(Evenement evenement);
        public bool VerwijderenEvenement(int id);
    }
}
