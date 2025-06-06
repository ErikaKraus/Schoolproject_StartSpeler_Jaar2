using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kassa.Data;

namespace Kassa.ViewModels
{
    public partial class EventgeschiedenisViewModel : BaseViewModel
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

        public ObservableCollection<Community> Communities { get; } = new ObservableCollection<Community>();

        [ObservableProperty]
        private Gebruiker _gebruiker;

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
                    OnPropertyChanged(nameof(ResterendePlaatsen)); 

                    EventGebruikersOphalen();
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
                        SelectedEvenement.CommunityId = value?.Id ?? 0; //CommunityId wordt geüpdatet
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

        [ObservableProperty]
        public ObservableCollection<EventGebruiker> eventGebruikers;

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

        public int AantalIngeschreven
        {
            get
            {
                if (SelectedEvenement == null || EventGebruikers == null)
                {
                    Debug.WriteLine("AantalIngeschreven: SelectedEvenement of EventGebruikers is null.");
                    return 0;
                }

                int totaalIngeschreven = SelectedEvenement.MaxDeelnemersEvent - ResterendePlaatsen;
                Debug.WriteLine($"AantalIngeschreven berekend: {totaalIngeschreven}");
                return totaalIngeschreven;
            }
        }




        public EventgeschiedenisViewModel(UserInformation userInformation, EventsRepository eventsRepository, CommunitiesRepository communitiesRepository, EventGebruikersRepository eventGebruikersRepository)
        {
            _userInformation = userInformation;
            _eventsRepository = eventsRepository;
            _communitiesRepository = communitiesRepository;
            _eventGebruikersRepository = eventGebruikersRepository;
            Evenementen = new ObservableCollection<Evenement>();
            FilteredEvenementen = Evenementen;
            AlleEventsOoitOphalen();
            CommunitiesOphalen();
            SelectedEvenement = new Evenement();

            Title = "Overzicht van alle evenementen ooit";

            OnAppearing();

        }

        public void OnAppearing()
        {
            UserId = _userInformation.LoggedInUser.Id;
            Username = "User: " + _userInformation.LoggedInUser.UserName;
        }

        private void AlleEventsOoitOphalen()
        {
            Debug.WriteLine("AlleEventsOoitOphalen methode gestart...");

            Evenementen.Clear();
            var evenementen = _eventsRepository.OphalenEvents();
            Debug.WriteLine($"Aantal evenementen opgehaald: {evenementen.Count()}");

            foreach (var evenement in evenementen)
            {
                Evenementen.Add(evenement);
            }

            Debug.WriteLine("Alle evenementen toegevoegd aan de collectie...");
        }

        private async Task CommunitiesOphalen()
        {
            Debug.WriteLine("CommunitiesOphalen methode gestart...");

            var communityList = await Task.Run(() => _communitiesRepository.OphalenCommunities());
            Debug.WriteLine($"Aantal communities opgehaald: {communityList.Count()}");

            Communities.Clear();
            Communities.Add(new Community { Id = 0, Naam = "Alle communities" }); //Alle communities toegevoegd, zodat alle communities weergegeven kunnen worden.
            foreach (var community in communityList)
            {
                Communities.Add(community);
            }
            Debug.WriteLine("Communities toegevoegd aan de collectie...");
        }

        private void VoegToeEnSorteer(Evenement nieuwEvenement)
        {
            evenementen.Add(nieuwEvenement);
            var gesorteerdeEvenementen = evenementen.OrderBy(e => e.Datum).ToList();
            evenementen.Clear();
            foreach (var evenement in gesorteerdeEvenementen)
            {
                evenementen.Add(evenement);
            }
        }

        private void FilterEvenementen()
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

            if (SelectedCommunity == null || SelectedCommunity.Id == 0)
            {
                FilteredEvenementen = new ObservableCollection<Evenement>(Evenementen);
                Debug.WriteLine($"Showing all events: {FilteredEvenementen.Count}");
            }
            else
            {
                FilteredEvenementen = new ObservableCollection<Evenement>(Evenementen.Where(e => e.CommunityId == SelectedCommunity.Id));
                Debug.WriteLine($"Events for Community {SelectedCommunity.Naam}: {FilteredEvenementen.Count}");
            }
        }

        private void ResetSelectedEvenement()
        {
            SelectedEvenement = new Evenement
            {
                Datum = DateTime.Now // Stel de datum in op de huidige datum
            };
            OnPropertyChanged(nameof(AantalIngeschreven)); //AantalIngeschreven wordt bijgewerkt
        }

        

        [RelayCommand]
        public void Deselecteren()
        {
            SelectedCommunity = Communities.FirstOrDefault(c => c.Id == 0);
            ResetSelectedEvenement(); //Geselecteerde evenement wordt gereset
            AlleEventsOoitOphalen();
            ApplyFilterToEvenementen();
        }

        [RelayCommand]
        public void ToonAlleEventsOoit()
        {
            AlleEventsOphalenGesorteerd();
        }

        //[RelayCommand]
        //public async Task Logout()
        //{
        //    _userInformation.Logout();
        //    await Shell.Current.GoToAsync("//LoginschermPage");
        //}

        //[RelayCommand]
        //public async Task GoHome()
        //{
        //    await Shell.Current.GoToAsync("//HomePage");
        //}

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

        private void AlleEventsOphalenGesorteerd()
        {
            Debug.WriteLine("AlleEventsOphalenGesorteerd methode gestart...");

            Evenementen.Clear();
            var evenementen = _eventsRepository.OphalenEvents();
            Debug.WriteLine($"Aantal evenementen opgehaald: {evenementen.Count()}");

            var gesorteerdeEvenementen = evenementen.OrderBy(e => e.Datum).ToList();

            foreach (var evenement in gesorteerdeEvenementen)
            {
                Evenementen.Add(evenement);
            }

            FilteredEvenementen = new ObservableCollection<Evenement>(Evenementen);
            Debug.WriteLine("Alle evenementen toegevoegd aan de collectie en gesorteerd...");
        }


        private void EventGebruikersOphalen()
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

            OnPropertyChanged(nameof(EventGebruikers));
            OnPropertyChanged(nameof(ResterendePlaatsen));
            OnPropertyChanged(nameof(AantalIngeschreven));
            Debug.WriteLine("EventGebruikers opgehaald en PropertyChanged getriggerd voor ResterendePlaatsen en AantalIngeschreven.");
        }
     
    }
}
