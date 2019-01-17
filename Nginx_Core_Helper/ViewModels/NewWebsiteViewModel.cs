using Avalonia.Controls;
using Nginx_Core_Helper.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace Nginx_Core_Helper.ViewModels
{
    public class NewWebsiteViewModel: ReactiveObject
    {
        public string SiteFileName { get; set; } = "example-site";
        public string ServerName { get; set; } = "example.com *.example.com";
        public string Description { get; set; } = "My awesome site.";
        public string FilePath { get; set; } = "/var/www/example-site/example-site.dll";
        public string DotNetPort { get; set; } = "5000";

        public string ErrorMessage { get; set; }

        public bool IsErrorMessageVisible { get; set; }

        public ICommand CloseCommand
        {
            get
            {
                return new Executor((param) =>
                {
                    (param as Window).Close();
                });
            }
        }

        public ICommand OKCommand
        {
            get
            {
                return new Executor((param) =>
                {
                    var siteConfig = $@"server {{
    listen        80;
    server_name   {ServerName};
    location / {{
        proxy_pass          http://localhost:{DotNetPort};
        include             /etc/nginx/proxy_params;
        proxy_http_version  1.1;
        proxy_set_header    Upgrade $http_upgrade;
        proxy_set_header    Connection keep-alive;
        proxy_cache_bypass  $http_upgrade;
    }}
}}".Replace("\r\n", "\n");

                    var serviceUnit = $@"[Unit]
Description={Description}

[Service]
WorkingDirectory=/var/www/example-site
ExecStart=/usr/bin/dotnet {FilePath}

Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier={SiteFileName}
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:{DotNetPort}
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target".Replace("\r\n", "\n");

                    try
                    {
                        File.WriteAllText($"/etc/nginx/sites-available/{SiteFileName}", siteConfig);
                        File.WriteAllText($"/etc/systemd/system/{SiteFileName}.service", serviceUnit);
                        (param as Window).Close();
                    }
                    catch (DirectoryNotFoundException)
                    {
                        ErrorMessage = "Directory not found.  Are you using Nginx and systemd?";
                        IsErrorMessageVisible = true;
                        this.RaisePropertyChanged("ErrorMessage");
                        this.RaisePropertyChanged("IsErrorMessageVisible");
                    }
                    catch
                    {
                        ErrorMessage = "Error writing to file.  Are you root?";
                        IsErrorMessageVisible = true;
                        this.RaisePropertyChanged("ErrorMessage");
                        this.RaisePropertyChanged("IsErrorMessageVisible");
                    }
                });
            }
        }
    }
}
