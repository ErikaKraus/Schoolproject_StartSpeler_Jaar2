 using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using Kassa.Data;
using Kassa.ViewModels;
 

namespace Kassa
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();

            builder.Services.AddSingleton<EventsRepository>();
            builder.Services.AddSingleton<CommunitiesRepository>();
            builder.Services.AddSingleton<EventGebruikersRepository>();

            builder.Services.AddSingleton<UserInformation>();

            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<BestelmenuPage>();
            builder.Services.AddTransient<ToDoBestellingPage>();
            builder.Services.AddTransient<EventbeheerPage>();
            builder.Services.AddTransient<LoginschermPage>();
            builder.Services.AddTransient<RollenbeheerPage>();
            builder.Services.AddTransient<VoorraadbeheerPage>();
            builder.Services.AddTransient<ThemaPage>();
            builder.Services.AddTransient<SalesPage>();
            builder.Services.AddTransient<EventInschrijvingPage>();
            builder.Services.AddTransient<EventgeschiedenisPage>();
            builder.Services.AddTransient<KlantAfrekenenPage>();

            builder.Services.AddSingleton<BestelmenuViewModel>();
            builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddSingleton<ToDoBestellingViewModel>();
            builder.Services.AddSingleton<EventbeheerViewModel>();
            builder.Services.AddSingleton<LoginschermViewModel>();
            builder.Services.AddSingleton<RollenbeheerViewModel>();
            builder.Services.AddSingleton<VoorraadbeheerViewModel>();
            builder.Services.AddSingleton<BaseViewModel>();
            builder.Services.AddSingleton<ThemaViewModel>();
            builder.Services.AddSingleton<EventInschrijvingViewModel>();
            builder.Services.AddSingleton<EventgeschiedenisViewModel>();
            builder.Services.AddSingleton<KlantAfrekenenViewModel>();
            builder.Services.AddSingleton<SalesViewModel>();

#endif

            return builder.Build();
        }
    }
}

