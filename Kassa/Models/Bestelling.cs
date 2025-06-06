using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Models
{
    public partial class Bestelling
    {
        public int Id { get; set; }
        public DateTime Tijdstip { get; set; }
        public int Tafelnummer { get; set; }
        public bool IsGeleverd { get; set; }
        public bool IsBetaald { get; set; }
        public bool IsOberBestelling { get; set; }
        public string? Commentaar { get; set; }
        public int? GebruikerId { get; set; }     // FK

        // navigation
        public Gebruiker? Gebruiker { get; set; }
        public List<BestellingLijn>? Bestellijnen { get; set; }

        private IBestellingenRepository _bestellingenRepository;


        Bestelling()            // constructor
        {
            _bestellingenRepository = new BestellingenRepository();

        }                       

        //[RelayCommand]
        //public void Geleverd()
        //{
        //    var result = _bestellingenRepository.MarkerenGeleverd(this.Id);

        //    if (result)
        //    {
        //        Shell.Current.DisplayAlert("Correct", "Bestelling is dus geleverd;", "OK");
        //    }
        //    else
        //    {
        //        Shell.Current.DisplayAlert("Fout", "Nog niet gemarkeerd als geleverd!", "OK");
        //    }
        //}

        //[RelayCommand]
        //public void Cancel()
        //{
        //    // de bestelling + bijhorende bestelllijnen moeten verwijderd worden
        //    var result = _bestellingenRepository.CancelBestelling(this.Id);

        //    if (result)
        //    {
        //        Shell.Current.DisplayAlert("Correct", "Foute Bestelling is cancelled.", "OK");
        //    }
        //    else
        //    {
        //        Shell.Current.DisplayAlert("Fout", "geen cancel", "OK");
        //    }
        //}

    }
}