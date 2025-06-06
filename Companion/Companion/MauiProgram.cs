using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace Companion
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();

            builder.Services.AddHttpClient();


            builder.Services.AddTransient<StartschermPage>();
            builder.Services.AddTransient<BestelmenuPage>();
            builder.Services.AddTransient<EventkalenderPage>();
            builder.Services.AddTransient<AccountPage>();
            builder.Services.AddTransient<LoginschermPage>();
            builder.Services.AddTransient<RegistratiePage>();

            builder.Services.AddSingleton<AccountViewModel>();
            builder.Services.AddSingleton<BaseViewModel>();
            builder.Services.AddSingleton<BestelmenuViewModel>();
            builder.Services.AddSingleton<EventkalenderViewModel>();
            builder.Services.AddSingleton<LoginschermViewModel>();
            builder.Services.AddSingleton<RegistratieViewModel>();
            builder.Services.AddSingleton<StartschermViewModel>();

            builder.Services.AddSingleton<ApiService>();



#endif

            return builder.Build();
        }
    }
}
