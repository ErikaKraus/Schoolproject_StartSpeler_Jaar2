namespace Companion.Views;

public partial class RegistratiePage : ContentPage
{
    public RegistratiePage(RegistratieViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}