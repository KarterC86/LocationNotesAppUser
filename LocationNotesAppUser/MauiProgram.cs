﻿using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Maps;

namespace LocationNotesAppUser
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
                }).UseMauiMaps();

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddHostedService<LocationListener>();

            return builder.Build();
        }
    }
}
