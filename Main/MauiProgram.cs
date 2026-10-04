using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Services;
using FFImageLoading.Maui;
using Main.Converter;
using Main.ViewModels;
using Plugin.Maui.Audio;
using Repository.Dbo;
using Syncfusion.Maui.Toolkit.Hosting;

namespace Main
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionToolkit()
                .UseFFImageLoading()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold");
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", "FluentSystemIcons-Regular");
                    fonts.AddFont("fa-solid-900.ttf", "FontAwesome");
                    fonts.AddFont("MaterialSymbolsRounded.ttf", "MaterialSymbolsRounded");
                    fonts.AddFont("FluentSystemIcons-Filled.ttf", "FluentSystemIcons-Filled");
                    fonts.AddFont("SegoeUIEmoji.ttf", "SegoeUIEmoji");
                })
                .AddAudio(options =>
                {
#if ANDROID
                    options.AudioContentType = Android.Media.AudioContentType.Music;
                    options.AudioUsageKind = Android.Media.AudioUsageKind.Media;
#endif
                }); ;


            if (Application.Current!=null)

            {
                builder.Services.AddSingleton<IApplication>(Application.Current);
            }
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<IPopupService, PopupService>();
            builder.Services.AddSingleton<IAssetService, AssetService>();
            builder.Services.AddSingleton<IAudioService, AudioService>();
            builder.Services.AddSingleton<IAlertService, AlertService>();
            builder.Services.AddSingleton<DatabaseAccess>();
            builder.Services.AddSingleton<BoolToOpacityConverter>();
            builder.Services.AddSingleton<BoolToFontAttributesConverter>();
            builder.Services.AddSingleton<EqualsConverter>();

            builder.Services.AddTransient<MainViewModel>();

            return builder.Build();
        }
    }
}
