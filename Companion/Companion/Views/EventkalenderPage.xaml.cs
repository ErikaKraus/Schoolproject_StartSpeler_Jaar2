using CommunityToolkit.Maui.Views;
using Companion.ApiServices;

namespace Companion.Views;

public partial class EventkalenderPage : ContentPage
{
    public EventkalenderPage(ApiService apiService)
    {
        InitializeComponent();
        EventkalenderViewModel viewModel = new EventkalenderViewModel(apiService);
        this.BindingContext = viewModel;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await (BindingContext as EventkalenderViewModel).EventsOphalen();
    }

}