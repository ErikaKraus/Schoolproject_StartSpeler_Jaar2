using Kassa.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kassa.Data.Repository;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Kassa.Data;

namespace Kassa.ViewModels
{
    public partial class KlantAfrekenenViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public string? UserId;
        [ObservableProperty]
        public string? username;

        private UserInformation _userInformation;

        [ObservableProperty]
        public ObservableCollection<Gebruiker> gebruikers;

        [ObservableProperty]
        public ObservableCollection<BestellingLijn> bestellijnen;

        [ObservableProperty]
        public ObservableCollection<Artikel> artikels;

        private IGebruikersRepository _gebruikersRepository;
        private IBestellingenRepository _bestellingenRepository;
        private IBestellijnenRepository _bestellijnenRepository;

        private string _totaalBedragString;
        public string TotaalBedragString 
        { 
            get { return _totaalBedragString; }
            set
            {
                if (_totaalBedragString != value) 
                {
                    _totaalBedragString = value;
                    OnPropertyChanged(nameof(TotaalBedragString));
                }
            }
        }

        private string _afrekening;
        public string Afrekening
        {
            get { return _afrekening; }
            set
            {
                if (_afrekening != value)
                {
                    _afrekening = value;
                    OnPropertyChanged(nameof(Afrekening));
                }
            }
        }

        decimal totaalbedrag = 0;
 
        
        public string Zoekterm { get;set; } = string.Empty;
       
        public Gebruiker? selected_gebruiker { get; set; } = null;

        public KlantAfrekenenViewModel(UserInformation userInformation)                                            // constructor
        {
            _userInformation = userInformation;
            OnAppearing();
            Title = "Afrekenen";
        }

        public void OnAppearing()
        {
            _gebruikersRepository = new GebruikersRepository();
            _bestellingenRepository = new BestellingenRepository();
            _bestellijnenRepository = new BestellijnenRepository();
            UserId = _userInformation.LoggedInUser.Id;
            Username = "User: " + _userInformation.LoggedInUser.UserName;

        }
       // =====================================================================================
            [RelayCommand]
        public void ZoekGebruikers()
        {
            IsBusy = true;
            if (Zoekterm == string.Empty)
            {
                Gebruikers = new ObservableCollection<Gebruiker>(_gebruikersRepository.AlleGebruikersOphalen());
            } 
            else 
            {
                Gebruikers = new ObservableCollection<Gebruiker>(_gebruikersRepository.OphalenGebruikersZoekterm(Zoekterm));  
            }
            
            IsBusy = false;
        }

        [RelayCommand]
        public void GebruikerGeselecteerd()
        {
            IsBusy = true;
            if (selected_gebruiker != null)                 // anders reclameert die selected_gebruiker.Id
            {
                Bestellijnen = new ObservableCollection<BestellingLijn>(_bestellijnenRepository.OphalenBestellijnPerGebruiker(selected_gebruiker.Id));
                totaalbedrag = 0;
                Afrekening = string.Empty;
                foreach (var xyz in Bestellijnen)
                {
                    if(xyz.Artikel != null)
                    {
                        totaalbedrag += xyz.aantal * xyz.Artikel.Prijs;
                        Afrekening += xyz.aantal.ToString() + " * " + xyz.Artikel.Prijs.ToString() + " \t\t " + xyz.Artikel.Naam + "\n";
                    }
                }
                TotaalBedragString = totaalbedrag.ToString() + " €";
                IsBusy = false;
            }

        }

        [RelayCommand]
        public void MarkeerBetaald()
        {
            IsBusy = true;
            var result = _bestellingenRepository.MarkerenBetaald(selected_gebruiker.Id);            // die 3 is niet juist !
            if(result) 
            {
                Afrekening = string.Empty;
                TotaalBedragString = "Betaling OK";
            
            }
            
        }

        [RelayCommand]
        public async Task Logout()
        {
            _userInformation.Logout();
            Routing.RegisterRoute("LoginschermViewModel", typeof(LoginschermPage));
            await Shell.Current.GoToAsync("LoginschermViewModel");                          // volgens mij zit hier de fout .. nieuwe instantie loginscherm
        }

        [RelayCommand]
        public async Task GoHome()
        {
            Routing.RegisterRoute("HomeViewModel", typeof(HomePage));
            await Shell.Current.GoToAsync("HomeViewModel");
        }

    }
}
