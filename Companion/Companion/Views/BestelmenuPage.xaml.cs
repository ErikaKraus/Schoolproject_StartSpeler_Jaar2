using Companion.ApiServices;

namespace Companion.Views;

public partial class BestelmenuPage : ContentPage
{
    public BestelmenuPage(ApiService apiService)
    {
        InitializeComponent();
        BestelmenuViewModel viewModel = new BestelmenuViewModel(apiService);
        this.BindingContext = viewModel;
    }
}

