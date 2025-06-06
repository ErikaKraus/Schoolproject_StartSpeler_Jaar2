namespace Kassa
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(EventbeheerPage), typeof(EventbeheerPage));
            Routing.RegisterRoute(nameof(EventInschrijvingPage), typeof(EventInschrijvingPage));
            Routing.RegisterRoute(nameof(EventgeschiedenisPage), typeof(EventgeschiedenisPage));
            Routing.RegisterRoute(nameof(LoginschermPage), typeof(LoginschermPage));
            Routing.RegisterRoute(nameof(BestelmenuPage), typeof(BestelmenuPage));
            Routing.RegisterRoute(nameof(ToDoBestellingPage), typeof(ToDoBestellingPage));
            Routing.RegisterRoute(nameof(RollenbeheerPage), typeof(RollenbeheerPage));
            Routing.RegisterRoute(nameof(SalesPage), typeof(SalesPage));
            Routing.RegisterRoute(nameof(KlantAfrekenenPage), typeof(KlantAfrekenenPage));
            Routing.RegisterRoute(nameof(EventInschrijvingPage), typeof(EventInschrijvingPage));
            Routing.RegisterRoute(nameof(ThemaPage), typeof(ThemaPage));
        }
    }
}
