using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;


namespace Companion.ViewModels
{
    public partial class EventkalenderViewModel : BaseViewModel
    {

        private ApiService _apiService;

        [ObservableProperty]
        public ObservableCollection<Evenement> evenementen;
        public ObservableCollection<Community> Communities { get; set; } = new ObservableCollection<Community>();

        [ObservableProperty]
        public ObservableCollection<Gebruiker> gebruikers;

        [ObservableProperty]
        public ObservableCollection<EventGebruiker> eventGebruikers;

        [ObservableProperty]
        private ObservableCollection<EvenementGroup> groupedEvenemenenten;

        [ObservableProperty]
        private string? spelerInformatie;

        private Evenement _selectedEvenement;
        public Evenement SelectedEvenement
        {
            get => _selectedEvenement;
            set
            {
                if (_selectedEvenement != value)
                {
                    _selectedEvenement = value;
                    OnPropertyChanged(nameof(SelectedEvenement));
                    OnPropertyChanged(nameof(BeschikbarePlaatsen)); //aantal beschikbare plaatsen wordt geüpdatet
                    SpelerInformatie = string.Empty;

                }
            }
        }

        private Community _selectedCommunity;
        public Community SelectedCommunity
        {
            get { return _selectedCommunity; }
            set
            {
                if (_selectedCommunity != value)
                {
                    _selectedCommunity = value;
                    OnPropertyChanged(nameof(SelectedCommunity));
                    FilterEvenementen();
                    if (SelectedEvenement != null)
                    {
                        SelectedEvenement.CommunityId = value?.Id ?? 0;
                    }
                    OnPropertyChanged(nameof(FilteredEvenementen));
                    ApplyCommunityFilter();
                    GroupEventsByDate();
                }
            }
        }

        private ObservableCollection<Evenement> _filteredEvenementen;
        public ObservableCollection<Evenement> FilteredEvenementen
        {
            get { return _filteredEvenementen; }
            set
            {
                if (_filteredEvenementen != value)
                {
                    _filteredEvenementen = value;
                    OnPropertyChanged(nameof(FilteredEvenementen));
                }
            }
        }

        private Gebruiker _gebruiker;
        public Gebruiker Gebruiker
        {
            get => _gebruiker;
            set
            {
                _gebruiker = value;
                OnPropertyChanged(nameof(Gebruiker));
            }
        }

        public int BeschikbarePlaatsen
        {
            get
            {
                if (SelectedEvenement == null)
                {
                    Debug.WriteLine("BeschikbarePlaatsen: SelectedEvenement is null.");
                    return 0;
                }

                if (EventGebruikers == null)
                {
                    Debug.WriteLine("BeschikbarePlaatsen: EventGebruikers is null.");
                    return 0;
                }

                int totaalIngeschreven = EventGebruikers
           .Where(eg => eg.EvenementId == SelectedEvenement.Id)
           .Sum(eg => eg.AantalDeelnemers);

                if (SelectedEvenement.MaxDeelnemersEvent == 0)
                {
                    Debug.WriteLine("MaxDeelnemersEvent is 0.");
                    return 0;
                }

                int beschikbarePlaatsen = SelectedEvenement.MaxDeelnemersEvent - totaalIngeschreven;
                Debug.WriteLine($"BeschikbarePlaatsen berekend: {beschikbarePlaatsen} (Max: {SelectedEvenement.MaxDeelnemersEvent}, Ingeschreven: {totaalIngeschreven})");

                return beschikbarePlaatsen;
            }
        }

        private bool _isPopupOpen;
        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set
            {
                _isPopupOpen = value;
                OnPropertyChanged(nameof(IsPopupOpen));
            }
        }

        

        public EventkalenderViewModel(ApiService apiService)
        {
            _apiService = apiService;

            Evenementen = new ObservableCollection<Evenement>();
            FilteredEvenementen = Evenementen; // Standaard alle evenementen laden
            Gebruiker = new Gebruiker();
            EventGebruikers = new ObservableCollection<EventGebruiker>(); 
            GroupedEvenemenenten = new ObservableCollection<EvenementGroup>();

            EventsOphalen();
            CommunitiesOphalen();
            BerekenBeschikbarePlaatsen();
            ApplyFilterToEvenementen();
            LoadUserData();

            SpelerInformatie = string.Empty;
        }

        private void GroupEventsByDate()
        {
            var grouped = Evenementen
                .GroupBy(e => e.Datum.Date)
                .Select(g => new EvenementGroup(g.Key, g.ToList()));

            GroupedEvenemenenten = new ObservableCollection<EvenementGroup>(grouped);
        }


        public async Task EventsOphalen()
        {
            Debug.WriteLine("Loading evenementen...");
            // Haal de evenementen op in een achtergrondthread
            var evenementen = await Task.Run(async () => await _apiService.GetEvenementenAsync());
            Debug.WriteLine($"Loaded {evenementen.Count()} evenementen.");

            // Werk de UI bij op de hoofdthread
            await MainThread.InvokeOnMainThreadAsync(() =>

            {
                Evenementen.Clear();
                foreach (var evenement in evenementen)
                {
                    // Controleer of Community correct is ingesteld
                    evenement.Community = Communities.FirstOrDefault(c => c.Id == evenement.CommunityId);
                    if (evenement.Community != null)
                    {
                        Debug.WriteLine($"Evenement: {evenement.Naam}, Community: {evenement.Community.Naam}, AfbeeldingPad: {evenement.Community.AfbeeldingPad}");
                    }
                    Evenementen.Add(evenement);
                }
            });

            await EventGebruikersOphalen(); // Load EventGebruikers after loading Evenementen
            BerekenBeschikbarePlaatsen();
            GroupEventsByDate();
        }
       

        public async Task EventGebruikersOphalen()
        {
            Debug.WriteLine("Loading event gebruikers...");
            var eventGebruikers = await Task.Run(async () => await _apiService.GetEventGebruikersAsync());
            Debug.WriteLine($"Loaded {eventGebruikers.Count()} event gebruikers.");

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                EventGebruikers.Clear();
                foreach (var eventGebruiker in eventGebruikers)
                {
                    EventGebruikers.Add(eventGebruiker);
                }
            });
        }

        public void BerekenBeschikbarePlaatsen()
        {
            foreach (var evenement in Evenementen)
            {
                int totaalIngeschreven = EventGebruikers
                    .Where(eg => eg.EvenementId == evenement.Id)
                    .Sum(eg => eg.AantalDeelnemers);
                evenement.BeschikbarePlaatsen = evenement.MaxDeelnemersEvent - totaalIngeschreven;
            }
        }

        public async Task CommunitiesOphalen()
        {
            Debug.WriteLine("Loading communities...");
            var communities = await _apiService.GetCommunitiesAsync();
            Debug.WriteLine($"Loaded {communities.Count()} communities.");

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Communities.Clear();
                foreach (var community in communities)
                {
                    Debug.WriteLine($"Community: {community.Naam}, AfbeeldingPad: {community.AfbeeldingPad}");
                    Communities.Add(community);
                }
            });
        }

        public void FilterEvenementen()
        {
            if (SelectedEvenement == null)
            {
                ApplyFilterToEvenementen();
            }
            else
            {
                FilteredEvenementen = new ObservableCollection<Evenement>(Evenementen.Where(e => e.Id == SelectedEvenement.Id));
            }
        }

        public void ApplyFilterToEvenementen()
        {
            Debug.WriteLine($"Filtering for Community ID: {SelectedCommunity?.Id}");

            //Belgische tijdzone
            TimeZoneInfo belgianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            DateTime nowInBelgianTime = TimeZoneInfo.ConvertTime(DateTime.Now, belgianTimeZone);

            if (SelectedCommunity == null || SelectedCommunity.Id == 0)
            {
                FilteredEvenementen = new ObservableCollection<Evenement>(
                    Evenementen.Where(e => TimeZoneInfo.ConvertTime(e.Datum, belgianTimeZone) >= nowInBelgianTime));
                Debug.WriteLine($"Showing all events: {FilteredEvenementen.Count}");
            }
            else
            {
                FilteredEvenementen = new ObservableCollection<Evenement>(
                    Evenementen.Where(e => TimeZoneInfo.ConvertTime(e.Datum, belgianTimeZone) >= nowInBelgianTime && e.CommunityId == SelectedCommunity.Id));
                Debug.WriteLine($"Events for Community {SelectedCommunity.Naam}: {FilteredEvenementen.Count}");
            }
        }

        public void ApplyCommunityFilter()
        {

            if (SelectedCommunity == null)
            {
                // Toon alle evenementen als er geen community is geselecteerd
                EventsOphalen();
            }
            else
            {
                var filteredEvents = Evenementen.Where(e => e.CommunityId == SelectedCommunity.Id).ToList();
                Evenementen.Clear();
                foreach (var evenement in filteredEvents)
                {
                    Evenementen.Add(evenement);
                }
            }
            GroupEventsByDate(); // Groepeer de gefilterde evenementen

        }

        [RelayCommand]
        public void ToonAlleEvents()
        {
            EventsOphalen();
            CommunitiesOphalen();
        }

        [RelayCommand]      
        public async Task Inschrijven()
        {
            Debug.WriteLine("Inschrijven command uitgevoerd");

            Debug.WriteLine($"SelectedEvenement: {SelectedEvenement?.Id}, GebruikerId: {Gebruiker?.Id}");
            if (SelectedEvenement != null && !string.IsNullOrEmpty(Gebruiker.Id))
            {
                Debug.WriteLine($"SelectedEvenement: {SelectedEvenement.Id}, GebruikerId: {Gebruiker.Id}");

                // Combineer de datum en het startuur van het evenement om een volledige DateTime te krijgen
                var evenementStartTijd = SelectedEvenement.Datum.Date + SelectedEvenement.Startuur;
                
                // Controleer of het startuur van het evenement is verstreken
                if (DateTime.Now >= evenementStartTijd)
                {
                    await Shell.Current.DisplayAlert("Fout", "Inschrijving is niet mogelijk, het evenement is al gestart.", "OK");
                    return;
                }

                //Check of gebruiker al geregistreerd is voor een evenement
                var alreadyRegistered = EventGebruikers.Any(eg => eg.EvenementId == SelectedEvenement.Id && eg.GebruikerId == Gebruiker.Id);
                if (alreadyRegistered)
                {
                    Debug.WriteLine("Gebruiker is al ingeschreven voor dit evenement.");
                    await Application.Current.MainPage.DisplayAlert("Error", "Je bent al ingeschreven voor dit evenement.", "OK");
                    return;
                }

                var eventGebruiker = new EventGebruiker
                {
                    EvenementId = SelectedEvenement.Id,
                    GebruikerId = Gebruiker.Id,
                    AantalDeelnemers = 1,
                    SpelerInformatie = SpelerInformatie
                };

                var jsonContent = JsonSerializer.Serialize(eventGebruiker);
                Debug.WriteLine($"JSON Payload: {jsonContent}");

                bool success = await _apiService.VoegEventGebruikerToeAsync(eventGebruiker);

                if (success)
                {
                    // Verwerk succesvolle inschrijving
                    Debug.WriteLine("Inschrijving succesvol");
                    await EventGebruikersOphalen(); // Haal de lijst van EventGebruikers opnieuw op
                    BerekenBeschikbarePlaatsen(); // Herbereken de beschikbare plaatsen
                    GroupEventsByDate(); // Update de gegroepeerde evenementen
                    OnPropertyChanged(nameof(BeschikbarePlaatsen)); // Update de beschikbare plaatsen in de pop-up
                    ClosePopup();
                }
                else
                {
                    // Verwerk fout bij inschrijving
                    Debug.WriteLine("Inschrijving mislukt");
                    await Application.Current.MainPage.DisplayAlert("Error", "Er is een fout opgetreden bij het inschrijven.", "OK");
                }
            }
            else
            {
                Debug.WriteLine("Inschrijven command mislukt: Vul alle velden in.");
                await Application.Current.MainPage.DisplayAlert("Error", "Vul alle velden in.", "OK");
            }
        }

        private async void LoadUserData()
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
                        Gebruiker = user;
                        Debug.WriteLine($"User loaded: {user.Id}, {user.Email}");
                    }
                }
            }
        }

        [RelayCommand]
        public void ShowPopup(Evenement evenement)
        {
            SelectedEvenement = evenement;
            IsPopupOpen = true;
        }

        [RelayCommand]
        public void ClosePopup()
        {
            IsPopupOpen = false;
        }
    }
}
