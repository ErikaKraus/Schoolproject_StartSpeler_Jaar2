namespace Kassa.Views;

public partial class SalesPage : ContentPage
{
	private SalesViewModel _vm;
    public SalesPage(SalesViewModel vm)
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