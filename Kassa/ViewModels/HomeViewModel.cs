using Kassa.Data;
using Kassa.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.ViewModels
{
    public partial class HomeViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private IAspnetuserrolesRepository _aspnetuserrolesRepository;
        public string? UserId;
      
        [ObservableProperty]
        public string? username;

        private UserInformation _userInformation;               // extra
        
        public bool beheerderAndCommunity { get; set; }
        public bool beheerderAndOber { get; set; }
        public bool onlyBeheerder { get; set; }

        [ObservableProperty]
        public ObservableCollection<AspnetUserRole> aspnetuserroles;

        public HomeViewModel(UserInformation userInformation )                                            // constructor
        {
            _userInformation = userInformation;
            OnAppearing();
        }

        public void OnAppearing()
        {
            //Title = "Home";
            beheerderAndCommunity = false;
            beheerderAndOber = false;
            onlyBeheerder = false;
            
            UserId = _userInformation.LoggedInUser.Id;
            Username = "User: " + _userInformation.LoggedInUser.UserName;

            _aspnetuserrolesRepository = new AspnetuserrolesRepository();
            if (UserId != null)
            {
                Aspnetuserroles = new ObservableCollection<AspnetUserRole>(_aspnetuserrolesRepository.OphalenUserRollenVoorLogin(UserId));
                foreach (var rol in Aspnetuserroles)
                {
                    if ((rol.RoleId == 4) || (rol.RoleId == 3)) { beheerderAndCommunity = true; };
                    if ((rol.RoleId == 4) || (rol.RoleId == 2)) { beheerderAndOber = true; };
                    if (rol.RoleId == 4) { onlyBeheerder = true; };
                }
            }
        }


        // ==================================================


        [RelayCommand]
        public async void GoToBestelmenu()
        {
            await Shell.Current.GoToAsync("//BestelmenuPage");
        }

        [RelayCommand]
        public async void GoToToDoBestelling()
        {
            await Shell.Current.GoToAsync("//ToDoBestellingPage");
        }

        [RelayCommand]
        public async void GoToKlantAfrekenen()
        {
            await Shell.Current.GoToAsync("//KlantAfrekenenPage");
        }

        [RelayCommand]
        public async void GoToEventbeheer()
        {
            await Shell.Current.GoToAsync("//EventbeheerPage");
        }

        [RelayCommand]
        public async void GoToEventInschrijving()
        {
            await Shell.Current.GoToAsync("//EventInschrijvingPage");
        }

        [RelayCommand]
        public async void GoToEventgeschiedenis()
        {
            await Shell.Current.GoToAsync("//EventgeschiedenisPage");
        }

        [RelayCommand]
        public async void GoToRollenbeheer()
        {
            await Shell.Current.GoToAsync("//RollenbeheerPage");
        }

        [RelayCommand]
        public async void GoToVoorraadbeheer()
        {
            await Shell.Current.GoToAsync("//VoorraadbeheerPage");
        }

        [RelayCommand]
        public async void GoToSales()
        {
            await Shell.Current.GoToAsync("//SalesPage");
        }

        [RelayCommand]
        public async Task Logout()
        {
            _userInformation.Logout();
            Routing.RegisterRoute("LoginSchermViewModel", typeof(LoginschermPage));
            await Shell.Current.GoToAsync("LoginSchermViewModel");
        }

        [RelayCommand]
        public async Task GoHome() {}


    }
}





