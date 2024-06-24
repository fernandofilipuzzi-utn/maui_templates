using Ejemplo3SQLite.DataAccess;
using Ejemplo3SQLite.ViewModels;
using Ejemplo3SQLite.Views;
using Microsoft.Extensions.Logging;

namespace Ejemplo3SQLite
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

            var dbContext = new EmpleadoDbContext();
            dbContext.Database.EnsureCreated();
            dbContext.Dispose();

            builder.Services.AddDbContext<EmpleadoDbContext>();

            builder.Services.AddTransient<EmpleadoPage>();
            builder.Services.AddTransient<EmpleadoViewModel>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainViewModel>();

            Routing.RegisterRoute(nameof(EmpleadoPage), typeof(EmpleadoPage));

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
