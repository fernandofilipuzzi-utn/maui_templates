using Microsoft.Extensions.Logging;

namespace AppEjemploLogin;

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
                fonts.AddFont("Font Awesome 6 Brands-Regular-400.otf", "AwesomeBrandRegular");
                fonts.AddFont("Font Awesome 6 Free-Regular-400.otf", "AwesomeRegular");
                fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "AwesomeSolid");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
