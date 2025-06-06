using Kassa.ViewModels;

namespace Kassa.Views;

public partial class EventgeschiedenisPage : ContentPage
{
    private EventgeschiedenisViewModel _vm;

    public EventgeschiedenisPage(EventgeschiedenisViewModel vm)
	{
		_vm = vm;
        InitializeComponent();
        BindingContext = vm;
    }
    

    private void OnCommunitySelected(object sender, EventArgs e)
    {
        var viewModel = this.BindingContext as EventgeschiedenisViewModel;
        viewModel?.ApplyFilterToEvenementen();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.OnAppearing();
    }
}