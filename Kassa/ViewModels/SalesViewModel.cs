using Kassa.Data;
using Kassa.Data.Repository;
using Kassa.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.ViewModels
{
    public partial class SalesViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private string _dagomzetString;
        private string _overzicht;
        private DateTime _dagtijdstip;

        public string? UserId;
        [ObservableProperty]
        public string? username;

        private IBestellijnenRepository _bestellijnenRepository;
        private UserInformation _userInformation;
        [ObservableProperty]
        public ObservableCollection<BestellingLijn> bestellijnen;
        public decimal OmzetPerDag = 0;
        public bool ZatAlInDeLijst = false;
        public string DagomzetString
        {
            get { return _dagomzetString; }
            set
            {
                if (_dagomzetString != value)
                {
                    _dagomzetString = value;
                    OnPropertyChanged(nameof(DagomzetString));
                }
            }
        }

        public string Overzicht
        {
            get { return _overzicht; }
            set
            {
                if (_overzicht != value)
                {
                    _overzicht = value;
                    OnPropertyChanged(nameof(Overzicht));
                }
            }
        }

        public DateTime Dagtijdstip
        {
            get { return _dagtijdstip; }
            set
            {
                if (_dagtijdstip != value)
                {
                    _dagtijdstip = value;
                    OnPropertyChanged(nameof(Dagtijdstip));
                }
            }
        }
        
        public SalesViewModel(UserInformation userInformation)                                            // constructor
        {
            _userInformation = userInformation;
            DagomzetString = "Totale Dagomzet = ";
            Dagtijdstip = DateTime.Now;
            _bestellijnenRepository = new BestellijnenRepository();
            Title = "Dagomzet";
            OnAppearing();

        }

        public void OnAppearing()
        {
            
            UserId = _userInformation.LoggedInUser.Id;
            Username = "User: " + _userInformation.LoggedInUser.UserName;
        }

            [RelayCommand]
        public void BerekenDagomzet()
        {
            IsBusy = true;
            // DagomzetString += " 50€";
            // Shell.Current.DisplayAlert("Wij berekenen", Dagtijdstip.ToString(), "OK");
            Bestellijnen = new ObservableCollection<BestellingLijn>(_bestellijnenRepository.OphalenBestellijnPerDag(Dagtijdstip));
            List<ArtikelAantalPrijs> DagLijstBestellingenPerArtikel = new List<ArtikelAantalPrijs>();

            foreach(var bestelling1 in Bestellijnen)
            {
                // als cola al in de de DaglijstBestellingPerArtikel staat, dan gewoon aantal verhogen, indien niet => cola achteraantoevoegen
                if(bestelling1 != null && bestelling1.Artikel != null && bestelling1.Artikel.Naam != null) 
                {
                    if (DagLijstBestellingenPerArtikel.Count == 0) 
                    {
                        ArtikelAantalPrijs ArtikelAantalPrijs1 = new ArtikelAantalPrijs(bestelling1.Artikel.Naam, bestelling1.aantal, bestelling1.Artikel.Prijs);
                        DagLijstBestellingenPerArtikel.Add(ArtikelAantalPrijs1);
                    }
                    else
                    {
                        foreach (var dagLijstBestellingArtikel in DagLijstBestellingenPerArtikel.ToList())
                        {
                            if (bestelling1.Artikel.Naam == dagLijstBestellingArtikel.NaamProduct)
                            {
                                dagLijstBestellingArtikel.AantalProduct += bestelling1.aantal;
                                ZatAlInDeLijst = true;
                            }
                        }
                        if(!ZatAlInDeLijst) 
                        {
                            ArtikelAantalPrijs ArtikelAantalPrijs1 = new ArtikelAantalPrijs(bestelling1.Artikel.Naam, bestelling1.aantal, bestelling1.Artikel.Prijs);
                            DagLijstBestellingenPerArtikel.Add(ArtikelAantalPrijs1);
                        }

                    }


                    
                    OmzetPerDag += bestelling1.Artikel.Prijs * bestelling1.aantal;
                }
            }
            foreach(var artikelType in DagLijstBestellingenPerArtikel)
            {
                Overzicht += artikelType.ToString() + "\n";
            }
            DagomzetString = "Dagomzet= " + OmzetPerDag.ToString();
        }

        [RelayCommand]
        public async Task Logout()
        {
            _userInformation.Logout();
            Routing.RegisterRoute("LoginschermViewModel", typeof(LoginschermPage));
            await Shell.Current.GoToAsync("LoginschermViewModel");
        }

        [RelayCommand]
        public async Task GoHome() 
        {
            Routing.RegisterRoute("HomeViewModel", typeof(HomePage));
            await Shell.Current.GoToAsync("HomeViewModel");
        }

    }
}
