using Companion.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Companion.ViewModels
{
    public partial class LoginschermViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;

        public LoginschermViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string email;

        

        [RelayCommand]
        public async Task Login()
        {
            Debug.WriteLine("Login command triggered.");

            var loginModel = new
            {
                Email = Email,
                Password = Password,
                RememberMe = false
            };

            var jsonContent = JsonSerializer.Serialize(loginModel);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                Debug.WriteLine("Login command triggered.");
                Debug.WriteLine("Attempting to log in...");
                Debug.WriteLine($"Sending POST request to api/AuthControllerAPI/login with content: {jsonContent}");
                
                var response = await _apiService.PostAsync("api/AuthControllerAPI/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Received response with status code: {response.StatusCode}");
                    Debug.WriteLine($"Response Body: {responseBody}");

                    var token = JsonConvert.DeserializeObject<JwtToken>(responseBody);
                    if (token == null || string.IsNullOrEmpty(token.Token))
                    {
                        throw new Exception("Invalid token received from API.");
                    }

                    //Save token
                    Preferences.Set("jwt_token", token.Token);
                    Debug.WriteLine("Token saved successfully.");


                    //Navigeer naar startscherm
                    Debug.WriteLine("Navigating to StartschermPage...");
                    await GoToStartschermAsync();
                }
                else
                {
                    //Toon error message
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Login failed: {errorMessage}");
                    await Application.Current.MainPage.DisplayAlert("Login mislukt", "Ongeldig e-mailadres of wachtwoord", "OK");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Debug.WriteLine($"Exception during login: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }      

     


        public class JwtToken
        {
            [JsonProperty("token")]
            public string Token { get; set; }
        }


        [RelayCommand]
        public static async Task GoToRegistratiePage()
        {
            await Shell.Current.GoToAsync("//RegistratiePage");
        }

        [RelayCommand]
        public async Task ForgotPassword()
        {
            await Application.Current.MainPage.DisplayAlert("Wachtwoord vergeten", "Stuur een e-mail voor een wachtwoordreset naar bram@startspeler.be", "OK");
        }

    }

}