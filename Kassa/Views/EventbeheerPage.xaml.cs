namespace Kassa.Views;

public partial class EventbeheerPage : ContentPage
{

    private EventbeheerViewModel _vm;

    public EventbeheerPage(EventbeheerViewModel vm)
	{
        _vm = vm;
		InitializeComponent();
        BindingContext = _vm;
    }

    private void OnCommunitySelected(object sender, EventArgs e)
    {
        var viewModel = this.BindingContext as EventbeheerViewModel;
        viewModel?.ApplyFilterToEvenementen();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.OnAppearing();
    }

}