using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Models
{
    public class BestellingInfo
    {
        public int BestellingId { get; set; }
        public int Tafelnummer { get; set; }
        public string? Voornaam { get; set; }

        public List<ArtikelAantalPrijs> ArtikelAantalPrijsX { get; set; }

        public BestellingInfo(int bestellingid, int tafelnummer,string voornaam, List<ArtikelAantalPrijs> lijstartikelaantalprijs)      // constructor
        { 
            this.BestellingId = bestellingid;
            this.Tafelnummer = tafelnummer;
            this.Voornaam = voornaam;
            this.ArtikelAantalPrijsX = lijstartikelaantalprijs;
        }

        public override string ToString()
        {
            string info = string.Empty;
            foreach(var artikelAantalPrijsY in ArtikelAantalPrijsX) 
            {
                info += artikelAantalPrijsY.NaamProduct + " * " + artikelAantalPrijsY.AantalProduct;
            }
            return "Tafel: " + Tafelnummer + " \tVoornaam " + Voornaam + " / " + info ;
        }


    }
}
