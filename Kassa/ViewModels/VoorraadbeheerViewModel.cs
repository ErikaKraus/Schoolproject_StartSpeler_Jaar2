using Kassa.Data;
using Kassa.Data.Repository;
using Kassa.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
// using Xamarin.KotlinX.Coroutines;

namespace Kassa.ViewModels
{
    public partial class VoorraadbeheerViewModel : BaseViewModel
    {
        public string ErrorMessage; 

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;

        [ObservableProperty]
        string title;

        [ObservableProperty]
        public string selectedArtikelType = string.Empty;

        [ObservableProperty]
        public Artikel selectedArtikel;

        [ObservableProperty]
        public string artikelNaam;

        [ObservableProperty]
        public int voorraadAantal;

        public string? UserId;
        [ObservableProperty]
        public string? username;
        private UserInformation _userInformation;

        public bool IsNotBusy => !IsBusy;

        [ObservableProperty]
        public ObservableCollection<Artikel> artikels;

        private IArtikelsRepository _artikelsRepository;

        public ObservableCollection<string> ArtikelTypes { get; set; } = new ObservableCollection<string>
        {
            { "Warme_Drank"},{"Koude_Drank"},{"Alcohol_Drank"},{"Snack"}
        };


        public VoorraadbeheerViewModel(UserInformation userInformation)                                            // constructor
        {
            _userInformation = userInformation;
            _artikelsRepository = new ArtikelsRepository();
            SelectedArtikel = new Artikel();
            OnAppearing();
            Title = "Voorraadbeheer";
        }


        public void OnAppearing()
        {
            
            UserId = _userInformation.LoggedInUser.Id;
            Username = "User: " + _userInformation.LoggedInUser.UserName;
        }
        // ===============================================================================

            [RelayCommand]
        public void Maakvoorraadlijst()
        {
            IsBusy = true;
            if (selectedArtikelType != string.Empty)
            {
                Artikels = new ObservableCollection<Artikel>(_artikelsRepository.OphalenArtikels(selectedArtikelType));
            } else 
            {
                Shell.Current.DisplayAlert("Fout", "Kies artikeltype", "OK");
            }
            IsBusy = false;
        }

        [RelayCommand]
        public void ArtikelGeselecteerd()
        {
            IsBusy = true;
            
            if(selectedArtikel.Prijs > 99) { selectedArtikel.Prijs = (decimal)selectedArtikel.Prijs / 100; };
            // speciale constructie voor als je veel select doet in artikels .. de decimal verspringt
            IsBusy = false;
        }


        [RelayCommand]
        public void MaakVeldenLeeg()
        {
            IsBusy = true;
            SelectedArtikel = new Artikel();                                             
            IsBusy = false;
        }

        [RelayCommand]
        public void MaakNieuwArtikel()
        {
            ValidateInput(selectedArtikel);
            if (ErrorMessage == string.Empty) 
            {
                var result = _artikelsRepository.ToevoegenArtikel(selectedArtikel);

                if (result)
                {
                    // selectie leegmaken + lijst artikels updaten
                    SelectedArtikel = new Artikel();
                    Artikels = new ObservableCollection<Artikel>(_artikelsRepository.OphalenArtikels(selectedArtikelType));
                }
                else
                {
                    Shell.Current.DisplayAlert("Fout", "Er is een fout opgetreden bij het aanpassen van het artikel", "OK");
                }

            }
            else 
            {
                Shell.Current.DisplayAlert("Fout", ErrorMessage, "OK");
            }
            
        }

        [RelayCommand]
        public void UpdateArtikel()
        {
            ValidateInput(selectedArtikel);
            if (ErrorMessage == string.Empty)
            {
                var result = _artikelsRepository.AanpassenArtikel(selectedArtikel);

                if (result)
                {
                    // selectie leegmaken + lijst artikels updaten
                    SelectedArtikel = new Artikel();
                    Artikels = new ObservableCollection<Artikel>(_artikelsRepository.OphalenArtikels(selectedArtikelType));
                }
                else
                {
                    Shell.Current.DisplayAlert("Fout", "Er is een fout opgetreden bij het aanpassen van het artikel", "OK");
                }

            } 
            else 
            {
                Shell.Current.DisplayAlert("Fout", ErrorMessage, "OK");

            }
            

        }

        [RelayCommand]
        public void DeleteArtikel()
        {
            var result = _artikelsRepository.VerwijderArtikel(selectedArtikel.Id);
           
            if (result)
            {
                // selectie leegmaken + lijst artikels updaten
                SelectedArtikel = new Artikel();
                Artikels = new ObservableCollection<Artikel>(_artikelsRepository.OphalenArtikels(selectedArtikelType));
            }
            else
            {
                Shell.Current.DisplayAlert("Fout", "Er is een fout opgetreden bij het verwijderen van het artikel", "OK");
            }

        }

        public void ValidateInput(Artikel artikel)
        {
            ErrorMessage = string.Empty;
            if (artikel.Naam == string.Empty) { ErrorMessage += "Vul ArtikelNaam in.\n"; };
            if (artikel.Voorraad < 0 || artikel.Voorraad >= 1000) { ErrorMessage += "Vul numerieke redelijke voorraad in.\n"; };
            if (artikel.Prijs < 0 || artikel.Prijs >= 100) { ErrorMessage += "Vul numerieke redelijke prijs in.\n"; };
            if (artikel.Type == null) { ErrorMessage += "Kies type.\n"; };

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
