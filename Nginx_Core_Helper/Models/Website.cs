using Nginx_Core_Helper.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace Nginx_Core_Helper.Models
{
    public class Website
    {
        public string ConfigFileName { get; set; }
        public string Port { get; set; }
        public bool IsNotRunning { get; set; }

        public ICommand DeleteCommand
        {
            get
            {
                return new Executor(param =>
                {
                    try
                    {
                        if (File.Exists($"/etc/nginx/sites-enabled/{ConfigFileName}"))
                        {
                            File.Delete($"/etc/nginx/sites-enabled/{ConfigFileName}");
                        }
                        if (File.Exists($"/etc/nginx/sites-available/{ConfigFileName}"))
                        {
                            File.Delete($"/etc/nginx/sites-available/{ConfigFileName}");
                        }
                        if (File.Exists($"/etc/systemd/system/{ConfigFileName}.service"))
                        {
                            File.Delete($"/etc/systemd/system/{ConfigFileName}.service");
                        }
                        MessageBus.Current.SendMessage("RefreshWebsites");
                    }
                    catch
                    {
                        Console.WriteLine("Delete failed.  Are you root?");
                    }
                });
            }
        }
    }
}
