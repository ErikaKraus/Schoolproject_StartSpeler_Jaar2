// using AudioUnit;
using Kassa.Data;
using Kassa.Data.Repository;
using Kassa.Models;
using Microsoft.Maui.Graphics.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.ViewModels
{
    public partial class ToDoBestellingViewModel : BaseViewModel
    {
        public string? UserId;
        [ObservableProperty]
        public string? username;
        [ObservableProperty]
        // public Microsoft.Maui.Graphics.Color achtergrondkleur = Azure;
        // public Color achtergrondkleur = "red";

        // private Timer timer;                                    // just testing

        private IBestellijnenRepository _bestellijnenRepository;
        private UserInformation _userInformation;


        [ObservableProperty]
        public ObservableCollection<BestellingLijn> bestellinglijnen;

        public ToDoBestellingViewModel(UserInformation userInformation)                    // constructor
        {
            _userInformation = userInformation;
            _bestellijnenRepository = new BestellijnenRepository();
            OnAppearing();
            Title = "Lopende Bestellingen";

        }

        public void OnAppearing()
        {
            UserId = _userInformation.LoggedInUser.Id;
            Username = "User: " + _userInformation.LoggedInUser.UserName;
            
            Bestellinglijnen = new ObservableCollection<BestellingLijn>(BestellijnenRepository.OphalenBestellijnenNietGeleverd());

            InitTimer();                    // verwijst naar onder
        }

            // ==================================================================


            public async void InitTimer()                         // youtube tim corey : periodic timer
        {
            using PeriodicTimer timer = new(TimeSpan.FromMilliseconds(20000));
            int teller = 0;

            while (await timer.WaitForNextTickAsync() && teller < 1000) 
            {
                teller++;
                _bestellijnenRepository = new BestellijnenRepository();
                Bestellinglijnen = new ObservableCollection<BestellingLijn>(_bestellijnenRepository.OphalenBestellijnenNietGeleverd());
            }
        }


        // de button press commando's zitten in klasse BestellingLijn.cs

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