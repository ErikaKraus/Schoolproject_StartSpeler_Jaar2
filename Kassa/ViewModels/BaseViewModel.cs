using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.ViewModels
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
        public static async Task GoToLoginschermAsync()
        {
            await Shell.Current.GoToAsync("//LoginschermPage");
        }

        ////HomePage
        //[RelayCommand]
        //public static async Task GoToHomeAsync()
        //{
        //    await Shell.Current.GoToAsync("//HomePage");
        //}

        //MainPage
        [RelayCommand]
        public async Task GoBack()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        //Voorraad
        [RelayCommand]
        public static async Task GoToVoorraadbeheerAsync()
        {
            await Shell.Current.GoToAsync("//VoorraadbeheerPage");
        }
        
        //Bestellen
        [RelayCommand]
        public static async Task GoToBestelmenuAsync()
        {
            await Shell.Current.GoToAsync("//BestelmenuPage");
        }

        [RelayCommand]
        public static async Task GoToKlantAfrekenenAsync()
        {
            await Shell.Current.GoToAsync("//KlantAfrekenenPage");
        }

        [RelayCommand]
        public static async Task GoToToDoBestellingAsync()
        {
            await Shell.Current.GoToAsync("//ToDoBestellingPage");
        }

        [RelayCommand]
        public static async Task GoToSalesAsync()
        {
            await Shell.Current.GoToAsync("//SalesPage");
        }

        //Events
        [RelayCommand]
        public static async Task GoToEventbeheerAsync()
        {
            await Shell.Current.GoToAsync("//EventbeheerPage");
        }

        [RelayCommand]
        public static async Task GoToEventInschrijvingAsync()
        {
            await Shell.Current.GoToAsync("//EventInschrijvingPage");
        }

        [RelayCommand]
        public static async Task GoToEventgeschiedenisAsync()
        {
            await Shell.Current.GoToAsync("//EventgeschiedenisPage");
        }

        [RelayCommand]
        public static async Task GoToDeelnamebeheerAsync()
        {
            await Shell.Current.GoToAsync("//DeelnamebeheerPage");
        }


        [RelayCommand]
        public static async Task GoToRollenbeheerAsync()                            
        {
            await Shell.Current.GoToAsync("//RollenbeheerPage");            
        }

        

       
               
    }
}
