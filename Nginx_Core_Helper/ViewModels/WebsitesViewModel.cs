using Avalonia.Controls;
using Nginx_Core_Helper.Models;
using Nginx_Core_Helper.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;

namespace Nginx_Core_Helper.ViewModels
{
    public class WebsitesViewModel : ReactiveObject
    {
        public WebsitesViewModel()
        {
            MessageBus.Current.Listen<string>()
                .Where(x => x == "RefreshWebsites")
                .Subscribe(x => GetAvailableWebsites());
            GetNginxStatus();
            GetAvailableWebsites();
        }

        public ICommand CreateNewWebsite
        {
            get
            {
                return new Executor(async (x) =>
                {
                    var newWindow = new NewWebsiteWindow();
                    await newWindow.ShowDialog();
                    GetAvailableWebsites();
                });
            }
        }
        public string NginxStatus { get; set; } = "Searching...";

        public bool IsInstallInfoVisible { get; set; }

        public bool IsErrorMessageVisible { get; set; }

        public string ErrorMessage { get; set; }

        public ObservableCollection<Website> WebsitesList { get; set; } = new ObservableCollection<Website>();

        public void GetAvailableWebsites()
        {
            WebsitesList.Clear();
            if (Directory.Exists("/etc/nginx/sites-available"))
            {
                foreach (var file in Directory.GetFiles("/etc/nginx/sites-available"))
                {
                    var port = File.ReadAllLines(file)
                                        ?.FirstOrDefault(x => x.Trim().StartsWith("proxy_pass"))
                                        ?.Trim()
                                        ?.Split(":")
                                        ?.Last()
                                        ?.Replace(";", string.Empty);
                    var website = new Website()
                    {
                        ConfigFileName = Path.GetFileName(file),
                        Port = port,
                        IsNotRunning = !File.Exists($"/etc/nginx/sites-enabled/{Path.GetFileName(file)}")
                    };
                    WebsitesList.Add(website);
                }
            }
            if (WebsitesList.Count == 0)
            {
                IsErrorMessageVisible = true;
                ErrorMessage = "No websites found.";
            }
            else
            {
                IsErrorMessageVisible = false;
            }
        }

        private void GetNginxStatus()
        {
            if (File.Exists("/usr/sbin/nginx"))
            {
                NginxStatus = "Installed";
            }
            else
            {
                NginxStatus = "Not Installed";
                IsInstallInfoVisible = true;
            }
        }
    }
}
