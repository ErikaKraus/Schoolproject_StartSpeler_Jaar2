using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.Json;
using System.Threading.Tasks;

namespace Companion.ViewModels
{
    public partial class StartschermViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;

        [ObservableProperty]
        private string userName;

        public StartschermViewModel(ApiService apiService)
        {
            _apiService = apiService;
            LoadUserData();
        }

        [RelayCommand]
        private async Task LoadUserData()
        {
            var token = Preferences.Get("jwt_token", string.Empty);
            if (!string.IsNullOrEmpty(token))
            {
                var response = await _apiService.GetUserDataAsync("api/AuthControllerAPI/userinfo", token);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var user = JsonSerializer.Deserialize<Gebruiker>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (user != null)
                    {
                        UserName = user.UserName;
                    }
                }
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
                //ResetUserDataFields();

                await Shell.Current.GoToAsync("//LoginschermPage");
            }
            else
            {
                Debug.WriteLine("Failed to logout from API.");
                await Application.Current.MainPage.DisplayAlert("Logout mislukt", "Kon niet uitloggen bij de server", "OK");
            }
        }

        //private void ResetUserDataFields()
        //{
        //    GebruikerId = string.Empty;
        //    UserName = string.Empty;
        //    Voornaam = string.Empty;
        //    Naam = string.Empty;
        //    Email = string.Empty;
        //    OnPropertyChanged(nameof(GebruikerId));
        //    OnPropertyChanged(nameof(UserName));
        //    OnPropertyChanged(nameof(Voornaam));
        //    OnPropertyChanged(nameof(Naam));
        //    OnPropertyChanged(nameof(Email));
        //}
    }
}
