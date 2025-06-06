using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companion.ViewModels
{
    public partial class BestelmenuViewModel : BaseViewModel
    {
        ApiService _apiService;

        private Artikel _selectArtikels;

        public ObservableCollection<string> KoudeDrank { get; set; } = new ObservableCollection<string>
        {
           {"Koude_Drank"}
        };
        public ObservableCollection<string> WarmeDrank { get; set; } = new ObservableCollection<string>
        {
           {"Warme_Drank"}
        };
        public ObservableCollection<string> AlcoholischeDrank { get; set; } = new ObservableCollection<string>
        {
           {"Alcohol_Drank"}
        };
        public ObservableCollection<string> Snacks { get; set; } = new ObservableCollection<string>
        {
           {"Snack"}
        };

        [ObservableProperty]
        public ObservableCollection<Artikel> artikels;

        public Artikel SelectedArtikel
        {
            get => _selectArtikels;
            set
            {
                if (_selectArtikels != value)
                {
                    _selectArtikels = value;
                    OnPropertyChanged(nameof(SelectedArtikel));

                }
            }
        }

        private ObservableCollection<Artikel> _filteredArtikels;
        public ObservableCollection<Artikel> FilteredArtikels
        {
            get { return _filteredArtikels; }
            set
            {
                if (_filteredArtikels != value)
                {
                    _filteredArtikels = value;
                    OnPropertyChanged(nameof(FilteredArtikels));
                }
            }
        }

        public BestelmenuViewModel(ApiService apiService)                                    // constructor
        {
            _apiService = apiService;


            artikels = new ObservableCollection<Artikel>();
            KoudeDrankOphalen();
            WarmeDrankOphalen();
            AlcoholischeDrankOphalen();
            SnacksOphalen();

        }

        [RelayCommand]
        public async Task KoudeDrankOphalen()
        {
            Debug.WriteLine("Loading artikels...");
            var artikels = await Task.Run(async () => await _apiService.GetKoudAsync());
            Debug.WriteLine($"Loaded {artikels.Count()} artikels.");

            await MainThread.InvokeOnMainThreadAsync(() =>

            {
                Artikels.Clear();
                foreach (var artikel in artikels)
                    Artikels.Add(artikel);

            });
        }

        [RelayCommand]
        public async Task WarmeDrankOphalen()
        {
            Debug.WriteLine("Loading artikels...");
            var artikels = await Task.Run(async () => await _apiService.GetWarmAsync());
            Debug.WriteLine($"Loaded {artikels.Count()} artikels.");

            await MainThread.InvokeOnMainThreadAsync(() =>

            {
                Artikels.Clear();
                foreach (var artikel in artikels)
                    Artikels.Add(artikel);
            });
        }

        [RelayCommand]
        public async Task AlcoholischeDrankOphalen()
        {
            Debug.WriteLine("Loading artikels...");
            var artikels = await Task.Run(async () => await _apiService.GetAlcoholischAsync());
            Debug.WriteLine($"Loaded {artikels.Count()} artikels.");

            await MainThread.InvokeOnMainThreadAsync(() =>

            {
                Artikels.Clear();
                foreach (var artikel in artikels)
                    Artikels.Add(artikel);
            });
        }

        [RelayCommand]
        public async Task SnacksOphalen()
        {
            Debug.WriteLine("Loading artikels...");
            var artikels = await Task.Run(async () => await _apiService.GetSnacksAsync());
            Debug.WriteLine($"Loaded {artikels.Count()} artikels.");

            await MainThread.InvokeOnMainThreadAsync(() =>

            {
                Artikels.Clear();
                foreach (var artikel in artikels)
                    Artikels.Add(artikel);
            });
        }
    }
}
