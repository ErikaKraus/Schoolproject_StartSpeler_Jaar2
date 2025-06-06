using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companion.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;

        [ObservableProperty]
        string title;

        public bool IsNotBusy => !IsBusy;

        [RelayCommand]
        public static async Task GoBack()
        {
            await Shell.Current.GoToAsync("//..");
        }

        [RelayCommand]
        public static async Task GoToBestelmenuAsync()
        {
            await Shell.Current.GoToAsync("//BestelmenuPage");
        }

        [RelayCommand]
        public static async Task GoToEventkalenderAsync()
        {
            await Shell.Current.GoToAsync("//EventkalenderPage");
        }

       

        [RelayCommand]
        public static async Task GoToAccountAsync()
        {
            await Shell.Current.GoToAsync("//AccountPage");
        }

        [RelayCommand]
        public static async Task GoToLoginschermAsync()
        {
            await Shell.Current.GoToAsync("//LoginschermPage");
        }

        [RelayCommand]
        public static async Task GoToRegistratieAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Navigating to RegistratiePage...");
                await Shell.Current.GoToAsync("//RegistratiePage");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception during navigation: {ex.Message}");
            }
        }

        [RelayCommand]
        public static async Task GoToStartschermAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Navigating to StartschermPage...");
                await Shell.Current.GoToAsync("//StartschermPage");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception during navigation: {ex.Message}");
            }
        }


    }
}
