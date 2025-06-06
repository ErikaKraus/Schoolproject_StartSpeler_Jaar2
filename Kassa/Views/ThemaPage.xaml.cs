namespace Kassa.Views;

public partial class ThemaPage : ContentPage
{
    ViewModels.ThemaViewModel ViewModel;

    public ThemaPage(ViewModels.ThemaViewModel viewModel)
    {
        InitializeComponent();

        ViewModel = viewModel;
        BindingContext = viewModel;
    }

    private void OnToggled(Object sender, ToggledEventArgs args)
    {
        if (!ViewModel.GetoggledDoorLaden)
        {
            ViewModel.ToggleTheme();
        }

        ViewModel.GetoggledDoorLaden = false;
    }
}