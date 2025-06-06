namespace Companion.Views;

public partial class LoginschermPage : ContentPage
{
	public LoginschermPage(LoginschermViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}