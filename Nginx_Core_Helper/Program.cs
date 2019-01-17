using System;
using Avalonia;
using Avalonia.Logging.Serilog;
using Nginx_Core_Helper.ViewModels;
using Nginx_Core_Helper.Views;

namespace Nginx_Core_Helper
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<MainWindow>(() => new MainWindowViewModel());
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToDebug();
    }
}
