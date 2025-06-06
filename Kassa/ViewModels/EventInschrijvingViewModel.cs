using Kassa.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.ViewModels
{
    public partial class EventInschrijvingViewModel : BaseViewModel
    {
        private EventsRepository _eventsRepository;
        private CommunitiesRepository _communitiesRepository;
        private EventGebruikersRepository _eventGebruikersRepository;
        private UserInformation _userInformation;

        public string? UserId;
        [ObservableProperty]
        public string? username;

        [ObservableProperty]
        public ObservableCollection<Evenement> evenementen;

        [ObservableProperty]
        public ObservableCollection<EventGebruiker> eventgebruikers;

        [ObservableProperty]
        public ObservableCollection<Gebruiker> gebruikers;

        [ObservableProperty]
        public ObservableCollection<EventGebruiker> eventGebruikers;

        [ObservableProperty]
        public ObservableCollection<Gebruiker> ingeschrevenGebruikers;

        private Gebruiker _selectedGebruiker;
        public Gebruiker SelectedGebruiker
        {
            get => _selectedGebruiker;
            set
            {
                if (SetProperty(ref _selectedGebruiker, value))
                {
                    // Controleer of de geselecteerde gebruiker niet null is en vul de velden
                    if (_selectedGebruiker != null)
                    {
                        Gebruiker = new Gebruiker
                        {
                            Id = _selectedGebruiker.Id,
                            Voornaam = _selectedGebruiker.Voornaam,
                            Naam = _selectedGebruiker.Naam
                        };
                    }
                }
            }
        }


        public ObservableCollection<Community> Communities { get; } = new ObservableCollection<Community>();

        private int _aantalDeelnemers;
        private string _waarschuwingstekst;
        private bool _toonWaarschuwing;

        public int AantalDeelnemers
        {
            get => _aantalDeelnemers;
            set
            {
                if (value > ResterendePlaatsen)
                {
                    Waarschuwingstekst = $"Het aantal beschikbare plaatsen is nog maar {ResterendePlaatsen}.";
                    ToonWaarschuwing = true;
                    //Het aantal in te geven deelnemers kan niet hoger gaan dan het aantal resterende plaatsen en wordt meteen aangepast naar het aantal resterende plaatsen
                    SetProperty(ref _aantalDeelnemers, ResterendePlaatsen);
                }

                else
                {
                    ToonWaarschuwing = false;
                    SetProperty(ref _aantalDeelnemers, value);
                }
            }
        }

        public int ResterendePlaatsen
        {
            get
            {
                if (SelectedEvenement == null || EventGebruikers == null)
                {
                    Debug.WriteLine("ResterendePlaatsen: SelectedEvenement of EventGebruikers is null.");
                    return 0;
                }

                int totaalIngeschreven = EventGebruikers
                    .Where(eg => eg.EvenementId == SelectedEvenement.Id)
                    .Sum(eg => eg.AantalDeelnemers);

                int resterendePlaatsen = SelectedEvenement.MaxDeelnemersEvent - totaalIngeschreven;
                Debug.WriteLine($"ResterendePlaatsen berekend: {resterendePlaatsen} (Max: {SelectedEvenement.MaxDeelnemersEvent}, Ingeschreven: {totaalIngeschreven})");

                return resterendePlaatsen;
            }
        }

        public string Waarschuwingstekst
        {
            get => _waarschuwingstekst;
            set => SetProperty(ref _waarschuwingstekst, value);
        }

        public bool ToonWaarschuwing
        {
            get => _toonWaarschuwing;
            set => SetProperty(ref _toonWaarschuwing, value);
        }


        private Evenement _selectedEvenement;
        public Evenement SelectedEvenement
        {
            get => _selectedEvenement;
            set
            {
                if (SetProperty(ref _selectedEvenement, value))
                {
                    // Zorgt ervoor dat de gerelateerde gegevens worden gereset elke keer wanneer een nieuw evenement wordt geselecteerd
                    Gebruiker = new Gebruiker();
                    AantalDeelnemers = 0;

                    EventGebruikersOphalen(); 

                    IngeschrevenGebruikersOphalen();

                    OnPropertyChanged(nameof(ResterendePlaatsen));
                    ValideerAantalDeelnemers();
                }
            }
        }

        
        [ObservableProperty]
        private Gebruiker _gebruiker;      

       

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
                        SelectedEvenement.CommunityId = value?.Id; 
                    }
                    OnPropertyChanged(nameof(FilteredEvenementen)); 
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
       

        public EventInschrijvingViewModel(EventsRepository eventsRepository, CommunitiesRepository communitiesRepository, 
            EventGebruikersRepository eventGebruikersRepository, UserInformation userInformation) // constructor
        {
            _eventsRepository = new EventsRepository();
            _communitiesRepository = new CommunitiesRepository();
            _eventGebruikersRepository = new EventGebruikersRepository();

            Evenementen = new ObservableCollection<Evenement>();
            FilteredEvenementen = new ObservableCollection<Evenement>();
            EventGebruikers = new ObservableCollection<EventGebruiker>();
            IngeschrevenGebruikers = new ObservableCollection<Gebruiker>();

            EventsOphalen();
            CommunitiesOphalen();
            ApplyFilterToEvenementen();
            EventGebruikersOphalen();

            _userInformation = userInformation;
            OnAppearing();
            Title = "Eventinschrijvingen";
        }

        public void OnAppearing()
        {
            UserId = _userInformation.LoggedInUser.Id;
            Username = "User: " + _userInformation.LoggedInUser.UserName;

        }

            public void OnSelectedGebruikerChanged(Gebruiker value)
        {
            if (value != null)
            {
                Gebruiker = new Gebruiker
                {
                    Id = value.Id,
                    UserName = value.UserName,
                    Voornaam = value.Voornaam,
                    Naam = value.Naam
                };
            }
        }

        partial void OnGebruikerChanged(Gebruiker value)
        {
            if (value != null)
            {
                AlleGebruikersVanEventOphalen();
            }
        }



        public void OnSelectedEvenementChanged(Evenement value)
        {
            if (value == null)
            {
                EventGebruikers.Clear();
            }
            else
            {
                EventGebruikersOphalen();
                IngeschrevenGebruikersOphalen();
            }

            // Roep ValideerAantalAantalDeelnemers op elke keer dat het geselecteerde evenement verandert om te verzekeren dat de UI juist wordt bijgewerkt met de nieuwe waarden.
            ValideerAantalDeelnemers();
            OnPropertyChanged(nameof(ResterendePlaatsen));  //Verzekert dat de UI zich bijwerkt als resterende plaatsen veranderen
        }


        public void OnAantalDeelnemersChanged(int value)
        {
            // Deze methode wordt automatisch gegenereerd en aangeroepen elke keer als AantalDeelnemers verandert.
            // Zorgt ervoor dat AantalDeelnemers nooit hoger is dan de ResterendePlaatsen.
            if (value > ResterendePlaatsen)
            {
                AantalDeelnemers = ResterendePlaatsen;
            }
        }

        private void ValideerAantalDeelnemers()
        {
            if (_aantalDeelnemers > ResterendePlaatsen)
            {
                Waarschuwingstekst = $"Het aantal beschikbare plaatsen is nog maar {ResterendePlaatsen}.";
                ToonWaarschuwing = true;
            }
            else
            {
                ToonWaarschuwing = false;
            }
        }

        public void EventsOphalen()
        {
            Debug.WriteLine("EventsOphalen methode gestart...");

            Evenementen.Clear();
            var evenementen = _eventsRepository.OphalenEvents();
            Debug.WriteLine($"Aantal evenementen opgehaald: {evenementen.Count()}");

            foreach (var evenement in evenementen)
            {
                if (evenement.Datum.Date >= DateTime.Today)
                {
                    Evenementen.Add(evenement);
                }
            }

            Debug.WriteLine("Evenementen toegevoegd aan de collectie...");
        }

        public async Task CommunitiesOphalen()
        {
            Debug.WriteLine("CommunitiesOphalen methode gestart...");

            var communityList = await Task.Run(() => _communitiesRepository.OphalenCommunities());
            Debug.WriteLine($"Aantal communities opgehaald: {communityList.Count()}");

            Communities.Clear();
            Communities.Add(new Community { Id = 0, Naam = "Alle communities" }); 
            foreach (var community in communityList)
            {
                Communities.Add(community);
            }
            Debug.WriteLine("Communities toegevoegd aan de collectie...");

        }

        public void ApplyFilterToEvenementen()
        {
            if (SelectedCommunity == null || SelectedCommunity.Id == 0)
            {
                FilteredEvenementen = new ObservableCollection<Evenement>(Evenementen.Where(e => e.Datum.Date >= DateTime.Today));
            }
            else
            {
                FilteredEvenementen = new ObservableCollection<Evenement>(Evenementen.Where(e => e.Datum.Date >= DateTime.Today && e.CommunityId == SelectedCommunity.Id));
            }
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

        [RelayCommand]
        public void AlleGebruikersVanEventOphalen()
        {
            if (SelectedEvenement == null)
            {
                return;
            }
            var eventGebruikerList = _eventGebruikersRepository.AlleGebruikersVanEventOphalen(SelectedEvenement);
            var gebruikersList = eventGebruikerList.Select(eventGebruiker => eventGebruiker.Gebruiker).ToList();
            Gebruikers = new ObservableCollection<Gebruiker>(gebruikersList);
        }

        public void IngeschrevenGebruikersOphalen()
        {
            if (SelectedEvenement == null)
            {
                IngeschrevenGebruikers.Clear();
                return;
            }          

            var eventGebruikerList = _eventGebruikersRepository.AlleGebruikersVanEventOphalen(SelectedEvenement);
            var gebruikersList = eventGebruikerList.Select(eventGebruiker =>
            {
                var gebruiker = eventGebruiker.Gebruiker;
                gebruiker.AantalDeelnemers = eventGebruiker.AantalDeelnemers;
                Debug.WriteLine($"Gebruiker {gebruiker.UserName}: AantalDeelnemers = {gebruiker.AantalDeelnemers}");
                return gebruiker;
            }).Distinct().ToList();

            // Log de ingeschreven gebruikers om te controleren
            Debug.WriteLine("Ingeschreven gebruikers na ophalen:");
            foreach (var gebruiker in IngeschrevenGebruikers)
            {
                Debug.WriteLine($"Ingeschreven gebruiker: {gebruiker.UserName} - {gebruiker.Voornaam} {gebruiker.Naam} - Aantal deelnemers: {gebruiker.AantalDeelnemers}");
            }

            IngeschrevenGebruikers = new ObservableCollection<Gebruiker>(gebruikersList);
            OnPropertyChanged(nameof(IngeschrevenGebruikers));
        }


        [RelayCommand]
        public void ToonAlleEvents()
        {
            EventsOphalen();
            CommunitiesOphalen();
        }

        [RelayCommand]
        public void ResetFormulier()
        {            
            Gebruiker = new Gebruiker();
            AantalDeelnemers = 0;
        }

        public void EventGebruikersOphalen()
        {
            if (SelectedEvenement == null)
            {
                EventGebruikers.Clear();
                Debug.WriteLine("Geen geselecteerd evenement, EventGebruikers geleegd.");

                return;
            }

            Debug.WriteLine("Loading event users for event: " + SelectedEvenement.Id);
            var eventGebruikers = _eventGebruikersRepository.AlleGebruikersVanEventOphalen(SelectedEvenement);
            EventGebruikers = new ObservableCollection<EventGebruiker>(eventGebruikers);

            // Trigger PropertyChanged om UI te updaten
            OnPropertyChanged(nameof(EventGebruikers));
            OnPropertyChanged(nameof(ResterendePlaatsen));
            Debug.WriteLine("EventGebruikers opgehaald en PropertyChanged getriggerd voor ResterendePlaatsen.");

        }             

 
        public void UpdateEventGebruikers()
        {
            OnPropertyChanged(nameof(EventGebruikers));
            OnPropertyChanged(nameof(ResterendePlaatsen));
            AantalDeelnemers = Math.Min(AantalDeelnemers, ResterendePlaatsen);
        }


        [RelayCommand]
        public async Task Uitschrijven()
        {
            if (SelectedGebruiker == null || SelectedEvenement == null)
            {
                await Shell.Current.DisplayAlert("Fout", "Selecteer een gebruiker en een evenement om te verwijderen.", "OK");
                return;
            }

            bool success = _eventGebruikersRepository.VerwijderEventGebruiker(SelectedGebruiker.Id, SelectedEvenement.Id);
            if (success)
            {
                EventGebruikersOphalen();
                IngeschrevenGebruikersOphalen();
                OnPropertyChanged(nameof(ResterendePlaatsen));

                //invoervelden worden leeggemaakt
                SelectedGebruiker = new Gebruiker();
                OnPropertyChanged(nameof(SelectedGebruiker));

                await Shell.Current.DisplayAlert("Succes", "Gebruiker succesvol uitgeschreven!", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Fout", "Er is een fout opgetreden bij het uitschrijven van de gebruiker.", "OK");
            }
        }

        [RelayCommand]
        public async Task Wijzigen()
        {
            if (SelectedGebruiker == null)
            {
                await Shell.Current.DisplayAlert("Fout", "Selecteer een gebruiker om te wijzigen.", "OK");
                return;
            }

            int origineleAantalDeelnemers = EventGebruikers
            .Where(eg => eg.GebruikerId == SelectedGebruiker.Id && eg.EvenementId == SelectedEvenement.Id)
            .Sum(eg => eg.AantalDeelnemers);

        int beschikbarePlaatsen = ResterendePlaatsen + origineleAantalDeelnemers;

        if (AantalDeelnemers > beschikbarePlaatsen)
        {
            await Shell.Current.DisplayAlert("Fout", $"Het aantal beschikbare plaatsen is nog maar {beschikbarePlaatsen}.", "OK");
            return;
        }

            if (string.IsNullOrEmpty(Gebruiker.Voornaam) || string.IsNullOrEmpty(Gebruiker.Naam))
            {
                await Shell.Current.DisplayAlert("Fout", "Zorg ervoor dat alle velden correct zijn ingevuld.", "OK");
                return;
            }

            SelectedGebruiker.Id = Gebruiker.Id;
            SelectedGebruiker.Voornaam = Gebruiker.Voornaam;
            SelectedGebruiker.Naam = Gebruiker.Naam;
            SelectedGebruiker.AantalDeelnemers = AantalDeelnemers;

            Debug.WriteLine($"Wijzigen gebruiker: {SelectedGebruiker.Id}");
            Debug.WriteLine($"Nieuwe voornaam: {SelectedGebruiker.Voornaam}");
            Debug.WriteLine($"Nieuwe naam: {SelectedGebruiker.Naam}");
            Debug.WriteLine($"Nieuw aantal deelnemers: {SelectedGebruiker.AantalDeelnemers}");

            bool success = _eventGebruikersRepository.WijzigGebruiker(SelectedGebruiker) && _eventGebruikersRepository.WijzigAantalDeelnemers(SelectedGebruiker, SelectedEvenement.Id);
            
            if (success)
            {
                IngeschrevenGebruikersOphalen();
                EventGebruikersOphalen();
                OnPropertyChanged(nameof(ResterendePlaatsen));


                await Shell.Current.DisplayAlert("Succes", "Gebruiker succesvol gewijzigd!", "OK");

                //lijst van ingeschreven gebruikers wordt bijgewerkt
                OnPropertyChanged(nameof(IngeschrevenGebruikers));

                // Update SelectedGebruiker in de lijst om wijzigingen weer te geven
                var index = IngeschrevenGebruikers.IndexOf(IngeschrevenGebruikers.First(g => g.Id == SelectedGebruiker.Id));
                if (index != -1)
                {
                    IngeschrevenGebruikers[index] = SelectedGebruiker;
                    OnPropertyChanged(nameof(IngeschrevenGebruikers));
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Fout", "Er is een fout opgetreden bij het wijzigen van de gebruiker.", "OK");
            }
        }

        [RelayCommand]
        public async Task Inschrijven()
        {
            Debug.WriteLine("Inschrijven start: EvenementId={0}, UserName={1}, AantalDeelnemers={2}", SelectedEvenement?.Id, Gebruiker?.UserName, AantalDeelnemers);

            if (SelectedEvenement != null && Gebruiker != null && !string.IsNullOrEmpty(Gebruiker.UserName) && AantalDeelnemers > 0)
            {
                // Combineer de datum en het startuur van het evenement om een volledige DateTime te krijgen
                var evenementStartTijd = SelectedEvenement.Datum.Date + SelectedEvenement.Startuur;
                
                // Controleer of het startuur van het evenement is verstreken
                if (DateTime.Now >= evenementStartTijd)
                {
                    await Shell.Current.DisplayAlert("Fout", "Inschrijving is niet mogelijk, het evenement is al gestart.", "OK");
                    return;
                }

                if (AantalDeelnemers > ResterendePlaatsen)
                {
                    await Shell.Current.DisplayAlert("Fout", $"Het aantal beschikbare plaatsen is nog maar {ResterendePlaatsen}.", "OK");
                    return;
                }

                var gebruiker = _eventGebruikersRepository.HaalGebruikerOpUserName(Gebruiker.UserName);
                if (gebruiker == null)
                {
                    await Shell.Current.DisplayAlert("Fout", "Gebruiker niet gevonden.", "OK");
                    return;
                }

                EventGebruiker nieuweEventGebruiker = new EventGebruiker
                {
                    GebruikerId = gebruiker.Id,
                    EvenementId = SelectedEvenement.Id,
                    AantalDeelnemers = AantalDeelnemers
                };

                bool success = _eventGebruikersRepository.VoegEventGebruikerToe(nieuweEventGebruiker);
                if (success)
                {
                    EventGebruikersOphalen();
                    IngeschrevenGebruikersOphalen();
                    Debug.WriteLine("Inschrijving succesvol. EvenementGebruikers opgehaald.");

                    OnPropertyChanged(nameof(ResterendePlaatsen));
                    await Shell.Current.DisplayAlert("Succes", "Inschrijving succesvol opgeslagen!", "OK");

                    Gebruiker = new Gebruiker();
                    AantalDeelnemers = 0;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Fout", "Er is een fout opgetreden bij het opslaan van de inschrijving.", "OK");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Fout", "Zorg ervoor dat alle velden correct zijn ingevuld.", "OK");
            }
            Debug.WriteLine("Inschrijven einde.");
        }




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
