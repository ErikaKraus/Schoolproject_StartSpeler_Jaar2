using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;



namespace Companion.ApiServices
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {

            _httpClient = new HttpClient { BaseAddress = new Uri("http://192.168.0.16:7153") };
        }

        public async Task<IEnumerable<Evenement>> GetEvenementenAsync()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var response = await _httpClient.GetAsync("api/EvenementControllerAPI");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var evenementen = JsonSerializer.Deserialize<IEnumerable<Evenement>>(responseBody, options);
            //enkel evenementen die nog moeten plaatsvinden
            evenementen = evenementen.Where(e => e.Datum >= DateTime.Today).ToList();
            //sorteren van oud naar nieuw
            evenementen = evenementen.OrderBy(e => e.Datum).ToList();
            // Log de gegevens voor verificatie
            if (evenementen != null)
            {
                foreach (var ev in evenementen)
                {
                    Debug.WriteLine($"Naam: {ev.Naam}, Afbeelding: {ev.Community?.Afbeelding}, Afbeeldingpad: {ev.Community?.AfbeeldingPad}, Datum: {ev.Datum}, Startuur: {ev.Startuur}");
                }
            }
            else
            {
                Debug.WriteLine("Deserialisatie mislukt!");
            }

            return evenementen;
        }

        #region artikels
        public async Task<IEnumerable<Artikel>> GetKoudAsync()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var response = await _httpClient.GetAsync("api/ArtikelControllerAPI");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var artikels = JsonSerializer.Deserialize<IEnumerable<Artikel>>(responseBody, options);

            artikels = artikels.Where(e => e.Type == "Koude_Drank").ToList();

            artikels = artikels.OrderBy(e => e.Naam).ToList();

            if (artikels != null)
            {
                foreach (var at in artikels)
                {
                    Debug.WriteLine($"Naam: {at.Naam}, Prijs: {at.Prijs}, Categorie: {at.Type}");
                }
            }
            else
            {
                Debug.WriteLine("Deserialisatie mislukt!");
            }

            return artikels;
        }

        public async Task<IEnumerable<Artikel>> GetWarmAsync()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var response = await _httpClient.GetAsync("api/ArtikelControllerAPI");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var artikels = JsonSerializer.Deserialize<IEnumerable<Artikel>>(responseBody, options);

            artikels = artikels.Where(e => e.Type == "Warme_Drank").ToList();

            artikels = artikels.OrderBy(e => e.Naam).ToList();

            if (artikels != null)
            {
                foreach (var at in artikels)
                {
                    Debug.WriteLine($"Naam: {at.Naam}, Prijs: {at.Prijs}, Categorie: {at.Type}");
                }
            }
            else
            {
                Debug.WriteLine("Deserialisatie mislukt!");
            }

            return artikels;
        }

        public async Task<IEnumerable<Artikel>> GetAlcoholischAsync()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var response = await _httpClient.GetAsync("api/ArtikelControllerAPI");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var artikels = JsonSerializer.Deserialize<IEnumerable<Artikel>>(responseBody, options);

            artikels = artikels.Where(e => e.Type == "Alcohol_Drank").ToList();

            artikels = artikels.OrderBy(e => e.Naam).ToList();

            if (artikels != null)
            {
                foreach (var at in artikels)
                {
                    Debug.WriteLine($"Naam: {at.Naam}, Prijs: {at.Prijs}, Categorie: {at.Type}");
                }
            }
            else
            {
                Debug.WriteLine("Deserialisatie mislukt!");
            }

            return artikels;
        }

        public async Task<IEnumerable<Artikel>> GetSnacksAsync()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var response = await _httpClient.GetAsync("api/ArtikelControllerAPI");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var artikels = JsonSerializer.Deserialize<IEnumerable<Artikel>>(responseBody, options);

            artikels = artikels.Where(e => e.Type == "Snack").ToList();

            artikels = artikels.OrderBy(e => e.Naam).ToList();

            if (artikels != null)
            {
                foreach (var at in artikels)
                {
                    Debug.WriteLine($"Naam: {at.Naam}, Prijs: {at.Prijs}, Categorie: {at.Type}");
                }
            }
            else
            {
                Debug.WriteLine("Deserialisatie mislukt!");
            }

            return artikels;
        }
        #endregion


        public async Task<IEnumerable<Community>> GetCommunitiesAsync()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var response = await _httpClient.GetAsync("api/CommunityControllerAPI");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<Community>>(responseBody, options);
        }


        public async Task<bool> VoegEventGebruikerToeAsync(EventGebruiker eventGebruiker)
        {
            try
            {
                var payload = new
                {
                    eventGebruiker.Id,
                    eventGebruiker.EvenementId,
                    eventGebruiker.GebruikerId,
                    eventGebruiker.AantalDeelnemers,
                    eventGebruiker.SpelerInformatie,
                };

                var jsonContent = JsonSerializer.Serialize(payload);
                Debug.WriteLine($"API Payload: {jsonContent}");
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/EventGebruikerControllerAPI", content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Inschrijving API-aanroep succesvol");
                    return true;
                }
                else
                {
                    Debug.WriteLine($"Inschrijving API-aanroep mislukt: {response.StatusCode} - {response.ReasonPhrase}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"API Error Content: {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Inschrijving API-aanroep exception: {ex.Message}");
                return false;
            }
        }


        public async Task<EventGebruiker> GetEventGebruikerByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/EventGebruiker/{id}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<EventGebruiker>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<EventGebruiker>> GetEventGebruikersAsync()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var response = await _httpClient.GetAsync("api/EventGebruikerControllerAPI"); 
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<EventGebruiker>>(responseBody, options);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, StringContent content)
        {           
            try
            {
                Debug.WriteLine($"Sending POST request to {url} with content: {content.ReadAsStringAsync().Result}");
                var response = await _httpClient.PostAsync(url, content);
                Debug.WriteLine($"Received response with status code: {response.StatusCode}");
                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception during POST request: {ex.Message}");
                throw;
            }
        }

        public async Task<HttpResponseMessage> GetUserDataAsync(string url, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetAsync(url);
        }

        public async Task<HttpResponseMessage> LogoutAsync()
        {
            var response = await _httpClient.PostAsync("api/AuthControllerAPI/logout", null);
            return response;
        }

    }
}