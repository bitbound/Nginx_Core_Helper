using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Nginx_Core_Helper.ViewModels;

namespace Nginx_Core_Helper.Views
{
    public class Websites : UserControl
    {
        public Websites()
        {
            this.DataContext = new WebsitesViewModel();
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
