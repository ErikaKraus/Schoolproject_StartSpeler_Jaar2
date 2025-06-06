using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Models
{
    public partial class BestellingLijn
    {
        public int Id { get; set; }
        public int aantal { get; set; }
        public int ArtikelId { get; set; }              // FK
        public int BestellingId { get; set; }           // FK

        // navigation
        public Artikel? Artikel { get; set; }
        public Bestelling? Bestelling { get; set; }

        private IBestellingenRepository _bestellingenRepository;

        BestellingLijn()            // constructor
        {
            _bestellingenRepository = new BestellingenRepository();
        }


        [RelayCommand]                              
        public void Geleverd()
        {
            if (this.Bestelling != null)
            {
                var result = _bestellingenRepository.MarkerenGeleverd(this.Bestelling.Id);

                if (result)
                {
                    Shell.Current.DisplayAlert("Correct", "Bestelling is dus geleverd;", "OK");
                    
                }
                else
                {
                    Shell.Current.DisplayAlert("Fout", "Nog niet gemarkeerd als geleverd!", "OK");
                }
            }


        }

        [RelayCommand]
        public void Cancel()
        {
            // de bestelling + bijhorende bestelllijnen moeten verwijderd worden
            if (this.Bestelling != null)
            {
                var result = _bestellingenRepository.CancelBestelling(this.Bestelling.Id);

                if (result)
                {
                    Shell.Current.DisplayAlert("Correct", "Foute Bestelling is cancelled.", "OK");
                }
                else
                {
                    Shell.Current.DisplayAlert("Fout", "geen cancel", "OK");
                }
            }



        }

    }
}