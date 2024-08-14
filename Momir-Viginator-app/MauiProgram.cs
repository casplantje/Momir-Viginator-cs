using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using MobilePrintinator;
using Momir_Viginator_app.ViewModels;
using Momir_Viginator_cs;

namespace Momir_Viginator_app
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMobilePrintinator()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var services = builder.Services;

            // Construct all important components
            // By constructing them here we can leverage dependency injection
            services.AddSingleton<ICardFactory>(serviceProvider => new OnlineScryfallFactory());

            services.AddSingleton<GeneratorViewModel>(serviceProvider => new GeneratorViewModel(serviceProvider.GetRequiredService<ICardFactory>()));
            services.addView<Generator, GeneratorViewModel>();

            services.AddSingleton<CardSearchViewModel>(serviceProvider => new CardSearchViewModel(serviceProvider.GetRequiredService<ICardFactory>()));
            services.addView<CardSearch, CardSearchViewModel>();

            services.AddSingleton<SettingsViewModel>(serviceProvider => new SettingsViewModel());
            services.addView<Settings, SettingsViewModel>();

            return builder.Build();
        }

        private static void addView<TView, TViewModel>(this IServiceCollection services)
            where TView : ContentPage, new()
        {
            services.AddSingleton<TView>(serviceProvider => new TView()
            {
                BindingContext = serviceProvider.GetRequiredService<TViewModel>()
            });
        }
    }
}
