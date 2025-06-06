// using Java.Nio.FileNio;
using Kassa.Data;
using Kassa.Data.Repository;
using Kassa.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XSystem.Security.Cryptography;                                    // NEW
using static Microsoft.Maui.Controls.VisualElement;

namespace Kassa.ViewModels
{
    public partial class RollenbeheerViewModel : BaseViewModel, INotifyPropertyChanged
    {
        [ObservableProperty]
        public ObservableCollection<Gebruiker> gebruikers;
        [ObservableProperty]
        public ObservableCollection<AspnetUserRole> aspnetuserroles;

        private UserInformation _userInformation;

        public string? UserId;
        [ObservableProperty]
        public string? username;

        private IGebruikersRepository _gebruikersRepository;
        private IAspnetuserrolesRepository _aspnetuserrolesRepository;

        public bool IsSpel { get; set; } = true;

        private bool _futureSpeler;
        public bool FutureSpeler
        {
            get { return _futureSpeler; }
            set
            {
                if (_futureSpeler != value)
                {
                    _futureSpeler = value;
                    OnPropertyChanged(nameof(FutureSpeler));
                }
            }
        }
        public bool CurrentSpeler { get; set; }
        private bool _futureOber;
        public bool FutureOber
        {
            get { return _futureOber; }
            set
            {
                if (_futureOber != value)
                {
                    _futureOber = value;
                    OnPropertyChanged(nameof(FutureOber));
                }
            }
        }
        public bool CurrentOber { get; set; }
        private bool _futureComMan;
        public bool FutureComMan
        {
            get { return _futureComMan; }
            set
            {
                if (_futureComMan != value)
                {
                    _futureComMan = value;
                    OnPropertyChanged(nameof(FutureComMan));
                }
            }
        }
        public bool CurrentComMan { get; set; }
        private bool _futureBeheerder;
        public bool FutureBeheerder
        {
            get { return _futureBeheerder; }
            set
            {
                if (_futureBeheerder != value)
                {
                    _futureBeheerder = value;
                    OnPropertyChanged(nameof(FutureBeheerder));
                }
            }
        }
        public bool CurrentBeheerder { get;set; }


        public Gebruiker? selected_gebruiker { get; set; } = null;

        [ObservableProperty]
        public string zoekterm;

        [ObservableProperty]
        public string newPassword;

        public RollenbeheerViewModel(UserInformation userInformation)                                            // constructor
        {
            _userInformation = userInformation;
            
            OnAppearing();
            Title = "Rollenbeheer";
        }
        public void OnAppearing()
        {
            UserId = _userInformation.LoggedInUser.Id;
            Username = "User: " + _userInformation.LoggedInUser.UserName;
            _gebruikersRepository = new GebruikersRepository();
            _aspnetuserrolesRepository = new AspnetuserrolesRepository();


        }
            // ============================================================

            [RelayCommand]
        public void ZoekUsers()
        {
            
            Gebruikers = new ObservableCollection<Gebruiker>(_gebruikersRepository.OphalenGebruikersZoekterm(Zoekterm));
            
            IsBusy = true;
        }

        [RelayCommand]
        public void GebruikerGeselecteerd()
        {
            IsBusy = true;
            if (selected_gebruiker != null)                 // anders reclameert die selected_gebruiker.Id
            {
                // Shell.Current.DisplayAlert("Fout", selected_gebruiker.Email, "OK");
                Aspnetuserroles = new ObservableCollection<AspnetUserRole>(_aspnetuserrolesRepository.OphalenUserRollen(selected_gebruiker.Id));
                FutureSpeler = false; FutureOber = false; FutureComMan = false; FutureBeheerder = false;
                CurrentSpeler = false; CurrentOber = false; CurrentComMan = false; CurrentBeheerder = false;
                foreach (var aspnetuserRole1 in Aspnetuserroles)
                {
                    if (aspnetuserRole1.RoleId == 1) { FutureSpeler = true; CurrentSpeler = true; };              // speler
                    if (aspnetuserRole1.RoleId == 2) { FutureOber = true; CurrentOber = true; };                // ober
                    if (aspnetuserRole1.RoleId == 3) { FutureComMan = true; CurrentComMan = true; };              // community manager
                    if (aspnetuserRole1.RoleId == 4) { FutureBeheerder = true; CurrentBeheerder = true; };           // beheerder
                }

            }

        }


        [RelayCommand]
        public void PasRollenAan()
        {
            if (selected_gebruiker != null) {
                if (selected_gebruiker.Id != null)
                {
                    // extra aspnetuserrole record aanmaken met RoleId = 1
                    if ((FutureSpeler != CurrentSpeler) && FutureSpeler) { var result = _aspnetuserrolesRepository.ToevoegenRol(selected_gebruiker.Id.ToString(), "1"); }
                    // aspnetuserrole record delete  met RoleId = 1
                    if ((FutureSpeler != CurrentSpeler) && !FutureSpeler) { var result = _aspnetuserrolesRepository.VerwijderRol(selected_gebruiker.Id.ToString(), "1"); }
                    // extra aspnetuserrole record aanmaken met RoleId = 2
                    if ((FutureOber != CurrentOber) && FutureOber) { var result = _aspnetuserrolesRepository.ToevoegenRol(selected_gebruiker.Id.ToString(), "2"); }
                    // aspnetuserrole record delete  met RoleId = 2
                    if ((FutureOber != CurrentOber) && !FutureOber) { var result = _aspnetuserrolesRepository.VerwijderRol(selected_gebruiker.Id.ToString(), "2"); }
                    // extra aspnetuserrole record aanmaken met RoleId = 3
                    if ((FutureComMan != CurrentComMan) && FutureComMan) { var result = _aspnetuserrolesRepository.ToevoegenRol(selected_gebruiker.Id.ToString(), "3"); }
                    // aspnetuserrole record delete  met RoleId = 3
                    if ((FutureComMan != CurrentComMan) && !FutureComMan) { var result = _aspnetuserrolesRepository.VerwijderRol(selected_gebruiker.Id.ToString(), "3"); }
                    // extra aspnetuserrole record aanmaken met RoleId = 4
                    if ((FutureBeheerder != CurrentBeheerder) && FutureBeheerder) { var result = _aspnetuserrolesRepository.ToevoegenRol(selected_gebruiker.Id.ToString(), "4"); }
                    // aspnetuserrole record delete  met RoleId = 4
                    if ((FutureBeheerder != CurrentBeheerder) && !FutureBeheerder) { var result = _aspnetuserrolesRepository.VerwijderRol(selected_gebruiker.Id.ToString(), "4"); }
                    Shell.Current.DisplayAlert("", "DONE ", "OK");
                }
            }

        }

        [RelayCommand]
        public void PasswordUpdate()
        {
            // https://www.phpbb.com/community/viewtopic.php?f=71&t=1771165
            // https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-8.0


            if (NewPassword != "" && NewPassword != null && selected_gebruiker.Id != null)
            {
                string passwordHashX = BCrypt.Net.BCrypt.HashPassword(NewPassword);

                var result = _gebruikersRepository.PasswordReset(selected_gebruiker, passwordHashX);

                if (result)
                {
                    NewPassword = "";
                }
                else
                {
                    Shell.Current.DisplayAlert("Fout", "Passwoord aanpassen niet gelukt.", "OK");
                }
                
            } else
            {
                Shell.Current.DisplayAlert("Let OP!", "Gelieve gebruiker te selecteren + nieuw password invullen. ", "OK");
            }

        }

        // ==========================================================

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
