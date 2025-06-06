namespace Companion
{
    public partial class App : Application
    {
        public static ApiService ApiService { get; private set; }

        public App()
        {
            InitializeComponent();

            ApiService = new ApiService();


            MainPage = new AppShell();

            // Standaard Loginscherm openen
            Shell.Current.GoToAsync("//LoginschermPage");
        }

             protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnSleep()
        {
            base.OnSleep();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }
    }
    }

