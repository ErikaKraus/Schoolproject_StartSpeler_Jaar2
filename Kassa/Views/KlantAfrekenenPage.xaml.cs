namespace Kassa.Views;

public partial class KlantAfrekenenPage : ContentPage
{
	private KlantAfrekenenViewModel _vm;
    public KlantAfrekenenPage(KlantAfrekenenViewModel vm)
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