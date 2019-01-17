using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Nginx_Core_Helper.Views
{
    public class NewWebsiteWindow : Window
    {
        public NewWebsiteWindow()
        {
            this.DataContext = new ViewModels.NewWebsiteViewModel();
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
