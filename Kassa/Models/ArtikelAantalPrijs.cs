using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Models
{
    public class ArtikelAantalPrijs
    {
        public string NaamProduct { get; set; }
        public int AantalProduct {  get; set; }
        public decimal PrijsProduct { get; set; }

        public ArtikelAantalPrijs(string naamproduct, int aantalProduct, decimal prijsProduct)                     // constructor
        { 
            this.NaamProduct = naamproduct;
            this.AantalProduct = aantalProduct;
            this.PrijsProduct = prijsProduct;
        }                           

        public override string ToString()
        {
            return "artikel: " + NaamProduct + " \t\tPrijs " + PrijsProduct + " * " + AantalProduct;
        }
    }
}
