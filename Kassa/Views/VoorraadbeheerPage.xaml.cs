namespace Kassa.Views;

public partial class VoorraadbeheerPage : ContentPage
{
    private VoorraadbeheerViewModel _vm;
    public VoorraadbeheerPage(VoorraadbeheerViewModel vm)
	{
        _vm = vm;
		InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.OnAppearing();
    }
}