using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// using Windows.Security.ExchangeActiveSyncProvisioning;

namespace Kassa.Models
{
    public class Artikel
    {
        public int Id { get; set; }
        public string? Naam { get; set; } = default!;
        public decimal Prijs { get; set; }
        public string? Type { get; set; }
        public string? Logo { get; set; }
        public string? Info { get; set; }
        public int Voorraad { get; set; }

        // navigation
        public List<BestellingLijn>? BestellingLijnen { get; set; }


        public Artikel() { }                           // constructor

        public Artikel(string naam, decimal prijs, string type, string logo, string info, int voorraad)
        {
            //Random r = new Random();
            //this.Id = r.Next(30, 999999);
            this.Naam = naam;
            this.Prijs = prijs;
            this.Type = type;
            this.Logo = logo;
            this.Info = info;
            this.Voorraad = voorraad;
        }
    }

}