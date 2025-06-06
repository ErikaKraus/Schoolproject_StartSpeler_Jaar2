namespace Companion.Views;

public partial class AccountPage : ContentPage
{
    private AccountViewModel _viewModel;

    public AccountPage(AccountViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _viewModel = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel != null)
        {
            _viewModel.LoadUserDataCommand.Execute(null);
        }
    }

}