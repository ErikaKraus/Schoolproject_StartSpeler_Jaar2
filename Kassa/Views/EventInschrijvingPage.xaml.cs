namespace Kassa.Views;

public partial class EventInschrijvingPage : ContentPage
{
    private EventInschrijvingViewModel _vm;
    public EventInschrijvingPage(EventInschrijvingViewModel vm)
	{
        _vm = vm;
		InitializeComponent();
        BindingContext = _vm;	
    }

    private void OnCommunitySelected(object sender, EventArgs e)
    {
        var viewModel = this.BindingContext as EventInschrijvingViewModel;
        viewModel?.ApplyFilterToEvenementen();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.OnAppearing();
    }
}