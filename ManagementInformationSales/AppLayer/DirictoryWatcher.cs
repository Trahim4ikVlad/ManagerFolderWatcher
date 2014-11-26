using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLayer
{
    public class DirictoryWatcher : IDisposable
    {
        public string CatalogName { get; set; }

        private FileSystemWatcher _catalogWatcher;

        private bool disposed = false;


        public void Init(string catalogName)
        {
            if (catalogName != null)
            {
                CatalogName = catalogName;
            }
            else
            {
                try
                {
                    var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    var settings = configFile.AppSettings.Settings;

                    if (settings["Catalog"] != null)
                    {
                        CatalogName = settings["Catalog"].Value;
                    }
                }
                catch (ConfigurationErrorsException)
                {
                    throw new Exception("Error reading app settings");
                }     
            }

            using (_catalogWatcher = new FileSystemWatcher(CatalogName))
            {
                _catalogWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                _catalogWatcher.Filter = "*.csv";
            }
        }

        public void Run()
        {
            _catalogWatcher.Changed += new FileSystemEventHandler(OnChanged);
            _catalogWatcher.Created += new FileSystemEventHandler(OnChanged);
            _catalogWatcher.Deleted += new FileSystemEventHandler(OnChanged);
            _catalogWatcher.Renamed += new RenamedEventHandler(OnRenamed);
            _catalogWatcher.EnableRaisingEvents = true;
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {

        }

        public void Stop()
        {
            _catalogWatcher.EnableRaisingEvents = false;
            _catalogWatcher.Dispose();
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {

        }

        public void Dispose()
        {
            _catalogWatcher.Changed -= OnChanged;
            _catalogWatcher.Created -= OnChanged;
            _catalogWatcher.Deleted -= OnChanged;
            _catalogWatcher.Renamed -= OnRenamed;
            _catalogWatcher.Dispose();
        }

    }
}
