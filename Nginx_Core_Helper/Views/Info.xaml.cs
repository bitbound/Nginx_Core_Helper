using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Nginx_Core_Helper.Views
{
    public class Info : UserControl
    {
        public Info()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
