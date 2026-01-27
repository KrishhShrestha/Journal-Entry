using Journal_Entry.Services;
using Journal_Entry.ViewModels;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using Syncfusion.Blazor;
using Syncfusion.Licensing;
using MudBlazor.Services;



namespace Journal_Entry
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();


            SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JGaF5cX2FCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdlWX5cdXVVRWhYUEJ+W0tWYEs=");

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSyncfusionBlazor();
            builder.Services.AddSingleton<SecurityService>();
            builder.Services.AddSingleton<PdfService>();

              

            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddScoped<PdfService>();
            builder.Services.AddMudServices();  




            //DATABASE SERVICE REGISTRATION
            string dbPath = Path.Combine(
                FileSystem.AppDataDirectory, "journal.db");

            builder.Services.AddSingleton(
                new DatabaseService(dbPath));

            //  VIEWMODEL REGISTRATION
            builder.Services.AddTransient<JournalViewModel>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
