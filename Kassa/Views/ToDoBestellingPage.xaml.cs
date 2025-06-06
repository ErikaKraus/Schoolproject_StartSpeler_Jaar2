namespace Kassa.Views;

public partial class ToDoBestellingPage : ContentPage
{
	private ToDoBestellingViewModel _vm;
    public ToDoBestellingPage(ToDoBestellingViewModel vm)
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