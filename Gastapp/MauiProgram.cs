using CommunityToolkit.Maui;
using Gastapp.BottomSheets;
using Gastapp.Data;
using Gastapp.Pages;
using Gastapp.Services.Navigation;
using Gastapp.Services.Spending;
using Gastapp.Services.User;
using Gastapp.ViewModels;
using Gastapp.ViewModels.Login;
using Gastapp.ViewModels.Today;
using Gastapp.ViewModels.TodaySpending;
using Gastapp.ViewModels.Welcome;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using The49.Maui.BottomSheet;

namespace Gastapp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseBottomSheet()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddDbContext<GastappDbContext>();

            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<LoginPage>();

            builder.Services.AddTransient<TodayViewModel>();
            builder.Services.AddTransient<TodayPage>();

            builder.Services.AddTransient<NewSpendingBottomSheet>();

            builder.Services.AddTransient<WelcomeViewModel>();
            builder.Services.AddTransient<Welcome>();


            builder.Services.AddTransient<TodaySpendingsViewModel>();
            builder.Services.AddTransient<TodaySpendingsPage>();

            builder.Services.AddSingleton<INavigationService, MauiNavigationService>();
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<SpendingService>();

            var dbContext = new GastappDbContext();
            dbContext.Database.EnsureCreated();
            dbContext.Dispose();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
