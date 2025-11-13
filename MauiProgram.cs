using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

namespace HAK_MAUI_Hybrid_Startertemplate
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            MauiAppBuilder builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
// Wird nur ausgeführt wenn Zielplattform MS Windows ist
#if WINDOWS
            SetWindowSizeAndPosition(builder, 800, 600);
#endif
// Wird nur ausgeführt wenn im Debug-Modus 
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void SetWindowSizeAndPosition(MauiAppBuilder builder, int widht, int height)
        {
            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddWindows(windowsLifecycleBuilder =>
                {
                    windowsLifecycleBuilder.OnWindowCreated(window =>
                    {
                        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                        var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
                        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

                        // Größe: 500x600
                        appWindow.Resize(new Windows.Graphics.SizeInt32(widht, height));

                        // Optional: Zentrieren
                        var displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(windowId, Microsoft.UI.Windowing.DisplayAreaFallback.Nearest);
                        if (displayArea != null)
                        {
                            var x = (displayArea.WorkArea.Width - widht) / 2;
                            var y = (displayArea.WorkArea.Height - height) / 2;
                            appWindow.Move(new Windows.Graphics.PointInt32(x, y));
                        }
                    });
                });
            });
        }
    }

    
}
