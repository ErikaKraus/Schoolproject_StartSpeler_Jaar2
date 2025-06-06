using Kassa.Data;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Kassa.ViewModels
{
    public partial class LoginschermViewModel : BaseViewModel
    {
        [ObservableProperty]
        public Gebruiker gebruiker;

        // bool ButtonPressed = false;
        private IGebruikersRepository _gebruikersRepository;

        private string _username;
        private string _password;
        private string _errormessage;
        private bool _showErrorMessage;

        private UserInformation _userInformation;

        public string Username
        {
            get { return _username; }
            set
           {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public string ErrorMessage
        {
            get { return _errormessage; }
            set
            {
                if (_errormessage != value)
                {
                    _errormessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }
        public bool ShowErrorMessage
        {
            get { return _showErrorMessage; }
            set
            {
                if (_showErrorMessage != value)
                {
                    _showErrorMessage = value;
                    OnPropertyChanged(nameof(ShowErrorMessage));
                }
            }
        }

        public LoginschermViewModel(UserInformation userInformation)            // constructor
        {
            _gebruikersRepository = new GebruikersRepository();
            
            _userInformation = userInformation;
            OnAppearing();
        }

        public void OnAppearing()
        {
            _userInformation.Logout();
            Username = string.Empty;
            Password = string.Empty;
        }

        [RelayCommand]
        public async void Login()
        {

            // dit is backdoor voor developers !
            if ( Username == "x" && Password == "x"  ) {
                _userInformation.Login(gebruiker);

                Routing.RegisterRoute("HomeViewModel", typeof(HomePage));
                await Shell.Current.GoToAsync("HomeViewModel");
                // knoppen enablen
                ShowErrorMessage = false;
            } 
            else {
                Gebruiker = new ObservableCollection<Gebruiker>(_gebruikersRepository.OphalenGebruikersUsername(Username)).FirstOrDefault();
                if ( Gebruiker == null ) { ErrorMessage = "gebruiker onbekend"; ShowErrorMessage = true; }
                else { 
                    if ( !BCrypt.Net.BCrypt.Verify(Password,Gebruiker.SimpleHash)    ) 
                        { ErrorMessage = "fout password"; ShowErrorMessage = true; } else
                            {

                            // Hier statische klasse invullen met logged in username
                            // LoggedInUser.UserId = Gebruiker.Id;
                            // LoggedInUser.UserName = Gebruiker.UserName;

                            _userInformation.Login(gebruiker);

                            ShowErrorMessage = false;
                        Routing.RegisterRoute("HomeViewModel", typeof(HomePage));
                        await Shell.Current.GoToAsync("HomeViewModel");
                            }
                
                }
                

            }



        }
    }
}
