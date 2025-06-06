namespace Kassa.Views;

public partial class BestelmenuPage : ContentPage
{
	private BestelmenuViewModel _vm;
    public BestelmenuPage(BestelmenuViewModel vm)
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