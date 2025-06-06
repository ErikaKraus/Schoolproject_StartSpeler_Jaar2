using Kassa.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.ViewModels
{
    public partial class EventbeheerViewModel : BaseViewModel
    {
        private EventsRepository _eventsRepository;
        private CommunitiesRepository _communitiesRepository;
        private UserInformation _userInformation;

        [ObservableProperty]
        public ObservableCollection<Evenement> evenementen;

        public ObservableCollection<Community> Communities { get; } = new ObservableCollection<Community>();

        [ObservableProperty]
        public Evenement selectedEvenement;
       
        public string? UserId;
        [ObservableProperty]
        public string? username;



        [ObservableProperty]
        public string actieLabel = "Nieuw event toevoegen";

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
                        SelectedEvenement.CommunityId = value?.Id ?? 0; // CommunityId wordt geüpdatet
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


        public EventbeheerViewModel(UserInformation userInformation, EventsRepository eventsRepository, CommunitiesRepository communitiesRepository) 
        {
            _eventsRepository = eventsRepository;
            _communitiesRepository = communitiesRepository;
            _userInformation = userInformation;

            Evenementen = new ObservableCollection<Evenement>();
            FilteredEvenementen = Evenementen; // Standaard alle evenementen laden
            EventsOphalen();
            _ = CommunitiesOphalen();
            SelectedEvenement = new Evenement();
            Title = "Event aanmaken";
            InitialiseerSelectedEvenement();

            OnAppearing();
        }


        public void OnAppearing()
        {
            UserId = _userInformation.LoggedInUser.Id;
            Username = "User: " + _userInformation.LoggedInUser.UserName;
        }

            //Datum van event staat standaard op vandaag
            private void InitialiseerSelectedEvenement()
        {
            SelectedEvenement = new Evenement
            {
                Datum = DateTime.Today
            };
        }

        private void EventsOphalen()
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

        private async Task CommunitiesOphalen()
        {
            Debug.WriteLine("CommunitiesOphalen methode gestart...");

            var communityList = await Task.Run(() => _communitiesRepository.OphalenCommunities());
            Debug.WriteLine($"Aantal communities opgehaald: {communityList.Count()}");

            Communities.Clear();
            Communities.Add(new Community { Id = 0, Naam = "Alle communities" }); //Extra optie toegevoegd zodat alle communities getoond worden
            foreach (var community in communityList)
            {
                Communities.Add(community);
            }
            Debug.WriteLine("Communities toegevoegd aan de collectie...");
        }

        partial void OnSelectedEvenementChanged(Evenement value)
        {
            if (value != null)
            {
                if (value.Id == 0)
                {
                    ActieLabel = "Nieuw evenement toevoegen";
                }
                else
                {
                    ActieLabel = "Evenement wijzigen/dupliceren/verwijderen";
                }
            }
        }


        //Sorteren van oud naar nieuw
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



        //Filteren op datum en community
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
                FilteredEvenementen = new ObservableCollection<Evenement>(Evenementen.Where(e => e.Datum.Date >= DateTime.Today));
                Debug.WriteLine($"Showing all events: {FilteredEvenementen.Count}");

            }
            else
            {
                FilteredEvenementen = new ObservableCollection<Evenement>(Evenementen.Where(e => e.Datum.Date >= DateTime.Today && e.CommunityId == SelectedCommunity.Id));
                Debug.WriteLine($"Events for Community {SelectedCommunity.Naam}: {FilteredEvenementen.Count}");

            }
        }


        //Actieknoppen
        [RelayCommand]
        public async Task Toevoegen()
        {
            if (string.IsNullOrWhiteSpace(SelectedEvenement.Naam))
            {
                await Shell.Current.DisplayAlert("Fout", "Vul de naam van het evenement in.", "OK");
                return;
            }

            if (SelectedEvenement.Datum == default)
            {
                await Shell.Current.DisplayAlert("Fout", "Vul de datum van het evenement in.", "OK");
                return;
            }

            if (SelectedEvenement.Startuur == default)
            {
                await Shell.Current.DisplayAlert("Fout", "Vul het startuur van het evenement in.", "OK");
                return;
            }

            if (SelectedEvenement.Einduur == default)
            {
                await Shell.Current.DisplayAlert("Fout", "Vul het einduur van het evenement in.", "OK");
                return;
            }

            if (SelectedEvenement.MaxDeelnemersEvent <= 0)
            {
                await Shell.Current.DisplayAlert("Fout", "Vul het aantal beschikbare plaatsen van het evenement in.", "OK");
                return;
            }

            if (SelectedCommunity == null || SelectedCommunity.Id == 0)
            {
                await Shell.Current.DisplayAlert("Fout", "Selecteer de community van het evenement.", "OK");
                return;
            }

            var result = _eventsRepository.ToevoegenEvenement(SelectedEvenement);

            if (result)
            {
                var nieuwEvenement = new Evenement
                {
                    Naam = SelectedEvenement.Naam,
                    Datum = SelectedEvenement.Datum,
                    Startuur = SelectedEvenement.Startuur,
                    Einduur = SelectedEvenement.Einduur,
                    Kostprijs = SelectedEvenement.Kostprijs,
                    CommunityId = SelectedEvenement.CommunityId,
                    MaxDeelnemersEvent = SelectedEvenement.MaxDeelnemersEvent,
                    ExtraInfo = SelectedEvenement.ExtraInfo
                };

                VoegToeEnSorteer(nieuwEvenement);
                SelectedEvenement = null;
                FilterEvenementen();

                InitialiseerSelectedEvenement();
            }
            else
            {
                await Shell.Current.DisplayAlert("Fout", "Er is een fout opgetreden bij het toevoegen van het evenement", "OK");
            }
        }


        [RelayCommand]
        public void Dupliceren()
        {
            if (SelectedEvenement != null)
            {
                var result = _eventsRepository.DuplicerenEvenement(SelectedEvenement);

                if (result)
                {
                    // Herlaad de evenementen om de duplicaat toe te voegen aan de lijst
                    EventsOphalen();
                }
                else
                {
                    Shell.Current.DisplayAlert("Fout", "Er is een fout opgetreden bij het dupliceren van het evenement", "OK");
                }
            }
            else
            {
                Shell.Current.DisplayAlert("Fout", "Geen evenement geselecteerd om te dupliceren", "OK");
            }
        }

        [RelayCommand]
        public void Bewerken()
        {
            var result = _eventsRepository.BewerkenEvenement(SelectedEvenement);

            if (result)
            {
                // Update het geselecteerde evenement in de Evenementen-collectie
                var evenementToUpdate = Evenementen.FirstOrDefault(e => e.Id == SelectedEvenement.Id);
                if (evenementToUpdate != null)
                {
                    // Vervang de oude versie door de bijgewerkte versie
                    int index = Evenementen.IndexOf(evenementToUpdate);
                    Evenementen[index] = SelectedEvenement;
                }

                InitialiseerSelectedEvenement();
            }
            else
            {
                Shell.Current.DisplayAlert("Fout", "Er is een fout opgetreden bij het bewerken van het evenement", "OK");
            }
        }

        [RelayCommand]
        public void Verwijderen()
        {
            var result = _eventsRepository.VerwijderenEvenement(SelectedEvenement.Id);

            if (result)
            {
                Evenementen.Remove(SelectedEvenement);
                InitialiseerSelectedEvenement();
            }
            else
            {
                Shell.Current.DisplayAlert("Fout", "Er is een fout opgetreden bij het verwijderen van het evenement", "OK");
            }
        }


        [RelayCommand]
        public void Deselecteren()
        {
            InitialiseerSelectedEvenement();
            SelectedCommunity = Communities.FirstOrDefault(c => c.Id == 0);
            EventsOphalen();
            ApplyFilterToEvenementen();
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