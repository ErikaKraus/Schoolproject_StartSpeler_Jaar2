using Kassa.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.ViewModels
{
    public partial class BestelmenuViewModel : BaseViewModel
    {
        public string? UserId;
        [ObservableProperty]
        public string? username;
        private UserInformation _userInformation;

        public ArtikelsRepository _artikelsRepository;

        #region artikels
        [ObservableProperty]
        private ObservableCollection<Artikel> koudeDranken;

        [ObservableProperty]
        private ObservableCollection<Artikel> warmeDranken;

        [ObservableProperty]
        private ObservableCollection<Artikel> alcoholischeDranken;

        [ObservableProperty]
        private ObservableCollection<Artikel> snacks;
        #endregion


        public BestelmenuViewModel(UserInformation userInformation)                                    // constructor
        {
            _userInformation = userInformation;
            OnAppearing();

            _artikelsRepository = new ArtikelsRepository();
            ToonKoudeDrank();
            ToonWarmeDrank();
            ToonAlcoholischeDrank();
            ToonSnacks();
        }

        [RelayCommand]
        public void ToonKoudeDrank()
        {
            koudeDranken = new ObservableCollection<Artikel>(_artikelsRepository.OphalenKoudeDrank());
        }

        [RelayCommand]
        public void ToonWarmeDrank()
        {
            warmeDranken = new ObservableCollection<Artikel>(_artikelsRepository.OphalenWarmeDrank());
        }

        [RelayCommand]
        public void ToonAlcoholischeDrank()
        {
            alcoholischeDranken = new ObservableCollection<Artikel>(_artikelsRepository.OphalenAlcoholischeDrank());
        }

        [RelayCommand]
        public void ToonSnacks()
        {
            snacks = new ObservableCollection<Artikel>(_artikelsRepository.OphalenSnacks());
        }

        public void OnAppearing()
        {
            UserId = _userInformation.LoggedInUser.Id;
            Username = "User: " + _userInformation.LoggedInUser.UserName;
            Title = "Bestelmenu";
        }

        // ==========================================================================

        [RelayCommand]
        public async Task Logout()
        {
            _userInformation.Logout();
            Routing.RegisterRoute("LoginSchermViewModel", typeof(LoginschermPage));
            await Shell.Current.GoToAsync("LoginSchermViewModel");
        }

        [RelayCommand]
        public async Task GoHome()
        {
            Routing.RegisterRoute("HomeViewModel", typeof(HomePage));
            await Shell.Current.GoToAsync("HomeViewModel");
        }

    }
}
