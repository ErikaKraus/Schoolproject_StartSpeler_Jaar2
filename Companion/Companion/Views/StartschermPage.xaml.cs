namespace Companion.Views;

public partial class StartschermPage : ContentPage
{
    private StartschermViewModel _viewModel;

    public StartschermPage(StartschermViewModel vm)
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