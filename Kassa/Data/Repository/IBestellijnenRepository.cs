using Kassa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Data.Repository
{
    public interface IBestellijnenRepository
    {
        // public IEnumerable<BestellingLijn> OphalenBestellinglijnDatum(DateTime datum1);
        public IEnumerable<BestellingLijn> OphalenBestellijnPerGebruiker(string gebruikerId);
        public IEnumerable<BestellingLijn> OphalenBestellijnPerDag(DateTime dag);

        public IEnumerable<BestellingLijn> OphalenBestellijnenNietGeleverd();

    }
}
