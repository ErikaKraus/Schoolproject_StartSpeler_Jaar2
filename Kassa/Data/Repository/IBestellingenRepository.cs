using Kassa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Data.Repository
{
    public interface IBestellingenRepository
    {
        public IEnumerable<Bestelling> OphalenBestellingen();
        public bool MarkerenBetaald(string gebruikerID);
        public bool MarkerenGeleverd(int BestellingId);
        public bool CancelBestelling(int BestellingId);
    }
}
