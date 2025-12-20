using Microsoft.Extensions.Logging;
using Journal_Entry.Services;
using Journal_Entry.ViewModels; 

namespace Journal_Entry
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
                });

            builder.Services.AddMauiBlazorWebView();

            // ✅ DATABASE SERVICE REGISTRATION
            string dbPath = Path.Combine(
                FileSystem.AppDataDirectory, "journal.db");

            builder.Services.AddSingleton(
                new DatabaseService(dbPath));

            // ✅ VIEWMODEL REGISTRATION
            builder.Services.AddTransient<JournalViewModel>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
