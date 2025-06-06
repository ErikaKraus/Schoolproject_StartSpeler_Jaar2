namespace Kassa.Views;

public partial class HomePage : ContentPage
{
    private HomeViewModel _vm;

	public HomePage(HomeViewModel vm)
	{
        _vm = vm;
		InitializeComponent();
        BindingContext = _vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.OnAppearing();
    }
}