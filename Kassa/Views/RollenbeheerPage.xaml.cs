namespace Kassa.Views;

public partial class RollenbeheerPage : ContentPage
{
	private RollenbeheerViewModel _vm;
    public RollenbeheerPage(RollenbeheerViewModel vm)
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