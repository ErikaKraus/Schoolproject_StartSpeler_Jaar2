using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Companion.ApiServices;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Companion.ViewModels
{
    public partial class RegistratieViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;        

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string confirmPassword;

        [ObservableProperty]
        private string naam;

        [ObservableProperty]
        private string voornaam;

        public RegistratieViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        private bool ValidatePassword(string password)
        {
            // Voorbeeld wachtwoordvereisten: minstens 8 tekens, één hoofdletter, één kleine letter, één cijfer en één speciaal teken.
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
            return regex.IsMatch(password);
        }

        [RelayCommand]
        public async Task Register()
        {
            if (Password != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Registratie mislukt", "Wachtwoorden komen niet overeen.", "OK");
                return;
            }

            if (!ValidatePassword(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Registratie mislukt", "Wachtwoord voldoet niet aan de vereisten (minimaal 8 tekens, waaronder één hoofdletter, één cijfer, één teken).", "OK");
                return;
            }


            var registerModel = new
            {
                UserName = UserName,
                Email = Email,
                Password = Password,
                Naam = Naam,
                Voornaam = Voornaam
            };

            var jsonContent = JsonSerializer.Serialize(registerModel);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                Debug.WriteLine("Attempting to register...");
                var response = await _apiService.PostAsync("api/AuthControllerAPI/register", content);

                if (response.IsSuccessStatusCode)
                {
                    // Registratie succesvol, navigate naar login page
                    Debug.WriteLine("Registratie is gelukt.");
                    await Shell.Current.GoToAsync("//LoginschermPage");
                }
                else
                {
                    // Show error message
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Registratie mislukt: {errorMessage}");
                    await Application.Current.MainPage.DisplayAlert("Registratie mislukt", errorMessage ?? "Niet mogelijk om te registeren", "OK");

                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Debug.WriteLine($"Exception tijdens registratie: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public class ErrorResponse
        {
            public List<IdentityError> Errors { get; set; }
        }

        public class IdentityError
        {
            public string Code { get; set; }
            public string Description { get; set; }
        }
    }

}
