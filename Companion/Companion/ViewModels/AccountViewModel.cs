using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companion.ViewModels
{
    public partial class AccountViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;       


        [ObservableProperty]
        private string gebruikerId;

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string voornaam;

        [ObservableProperty]
        private string naam;

        [ObservableProperty]
        private string email;

        

        public AccountViewModel(ApiService apiService)
        {
            _apiService = apiService;
            LoadUserData();
        }

        [RelayCommand]
        public async Task LoadUserData()
        {
            try
            {
                var token = Preferences.Get("jwt_token", string.Empty);
                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Token not found");
                }

                Debug.WriteLine($"Token: {token}");

                var response = await _apiService.GetUserDataAsync("api/AuthControllerAPI/userinfo", token);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Response Body: {responseBody}");
                    var user = JsonSerializer.Deserialize<Gebruiker>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (user != null)
                    {
                        GebruikerId = user.Id;
                        UserName = user.UserName;
                        Voornaam = user.Voornaam;
                        Naam = user.Naam;
                        Email = user.Email;
                    }
                }
                else
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Failed to load user data: {response.ReasonPhrase}, {errorBody}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception during LoadUserData: {ex.Message}");
            }
        }


        [RelayCommand]
        public async Task Logout()
        {
            var response = await _apiService.LogoutAsync();
            if (response.IsSuccessStatusCode)
            {
                //Reset bewaarde gegevens van vorige gebruiker
                Preferences.Clear();
                Debug.WriteLine("Token and other stored data removed successfully.");

                // Reset gebruikersgegevens
                ResetUserDataFields();

                await Shell.Current.GoToAsync("//LoginschermPage");
            }
            else
            {
                Debug.WriteLine("Failed to logout from API.");
                await Application.Current.MainPage.DisplayAlert("Logout mislukt", "Kon niet uitloggen bij de server", "OK");
            }
        }

        private void ResetUserDataFields()
        {
            GebruikerId = string.Empty;
            UserName = string.Empty;
            Voornaam = string.Empty;
            Naam = string.Empty;
            Email = string.Empty;
            OnPropertyChanged(nameof(GebruikerId));
            OnPropertyChanged(nameof(UserName));
            OnPropertyChanged(nameof(Voornaam));
            OnPropertyChanged(nameof(Naam));
            OnPropertyChanged(nameof(Email));
        }
    }
}
