using Avalonia;
using Avalonia.Markup.Xaml;

namespace Nginx_Core_Helper
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
