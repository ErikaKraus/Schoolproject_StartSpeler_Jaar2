using Kassa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Data.Repository
{
    public interface IArtikelsRepository
    {
        public IEnumerable<Artikel> OphalenArtikels(string artikeltype);

        public bool ToevoegenArtikel(Artikel artikel);
        public List<Artikel> OphalenArtikels2();
        //public Artikel OphalenArtikelPerId(int id);
        public bool AanpassenArtikel(Artikel artikel);
       

        public bool VerwijderArtikel(int id);

    }
}
