namespace Kassa.Views;

public partial class LoginschermPage : ContentPage
{
   private LoginschermViewModel _vm;

    public LoginschermPage(LoginschermViewModel vm)
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