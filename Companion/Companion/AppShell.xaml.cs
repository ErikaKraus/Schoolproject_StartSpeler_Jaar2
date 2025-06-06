namespace Companion
{
    public partial class AppShell : Shell
    {
       
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginschermPage), typeof(LoginschermPage));
            Routing.RegisterRoute(nameof(RegistratiePage), typeof(RegistratiePage)); 
            Routing.RegisterRoute(nameof(StartschermPage), typeof(StartschermPage));
            Routing.RegisterRoute(nameof(BestelmenuPage), typeof(BestelmenuPage));
            Routing.RegisterRoute(nameof(EventkalenderPage), typeof(EventkalenderPage));
            Routing.RegisterRoute(nameof(AccountPage), typeof(AccountPage));

            ////De standaard route is de startscherm
            //GoToAsync("//LoginschermPage");
        }
    }
}
